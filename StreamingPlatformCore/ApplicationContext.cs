using Microsoft.EntityFrameworkCore;
using StreamingPlatformCore.Entities;

namespace StreamingPlatformCore
{
    /// <summary>
    /// Основной контекст приложения для подключения к базе данных.
    /// Реализует паттерн Singleton для обеспечения единственного подключения.
    /// </summary>
    public class ApplicationContext : DbContext
    {
        /// <summary>
        /// Коллекция пользователей.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Коллекция каналов.
        /// </summary>
        public DbSet<StreamChannel> StreamChannels { get; set; }

        /// <summary>
        /// Коллекция трансляций.
        /// </summary>
        public DbSet<LiveStream> LiveStreams { get; set; }

        /// <summary>
        /// Коллекция подписок.
        /// </summary>
        public DbSet<Subscription> Subscriptions { get; set; }

        /// <summary>
        /// Коллекция донатов.
        /// </summary>
        public DbSet<Donation> Donates { get; set; }

        /// <summary>
        /// Коллекция сообщений чата.
        /// </summary>
        public DbSet<ChatMessage> ChatMessages { get; set; }

        /// <summary>
        /// Коллекция категорий.
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        #region Singleton
        private static ApplicationContext? _instance;

        /// <summary>
        /// Возвращает единственный экземпляр контекста (или создаёт новый, если не существует).
        /// </summary>
        /// <param name="forceInMemory">Если true, используется база данных в памяти (для тестов).</param>
        /// <param name="insertTestValues">Если true, заполняет базу тестовыми данными.</param>
        public static ApplicationContext GetInstance(bool forceInMemory = false, bool insertTestValues = false)
        {
            if (_instance == null)
            {
                _instance = new ApplicationContext(forceInMemory, insertTestValues);
            }
            return _instance;
        }

        /// <summary>
        /// Сбрасывает Singleton-экземпляр контекста (используется в тестах).
        /// </summary>
        public static void ResetInstance()
        {
            _instance?.Dispose();
            _instance = null;
        }
        #endregion

        /// <summary>
        /// Флаг использования базы данных в памяти.
        /// </summary>
        private readonly bool _forceInMemory;

        /// <summary>
        /// Приватный конструктор, создающий контекст с заданными параметрами.
        /// </summary>
        /// <param name="forceInMemory">Использовать InMemory-провайдер.</param>
        /// <param name="insertTestValues">Добавить тестовые данные после создания.</param>
        private ApplicationContext(bool forceInMemory = false, bool insertTestValues = false)
        {
            _forceInMemory = forceInMemory;

#if DEBUG
            // В отладочном режиме удаляем и пересоздаём базу для воспроизводимости
            Database.EnsureDeleted();
            Database.EnsureCreated();
#endif

            if (insertTestValues)
            {
                AddTestValues();
            }
        }

        /// <summary>
        /// Заполняет базу 32 тестовыми пользователями, каналами и стримами.
        /// </summary>
        private void AddTestValues()
        {
            for (int i = 0; i < 32; i++)
            {
                var user = new User($"test-user-{i}", "password");
                Users.Add(user);
                SaveChanges();
                var streamChannel = new StreamChannel($"test-streamchannel-{i}", "", user.Id);
                StreamChannels.Add(streamChannel);
                SaveChanges();
                var liveStream = new LiveStream()
                {
                    Title = $"test-livestream-{i}",
                    StreamChannelId = streamChannel.Id,
                    StartDate = DateTime.Now,
                    Duration = TimeSpan.FromHours(i),
                    Status = LiveStreamStatus.Scheduled
                };
                LiveStreams.Add(liveStream);
                SaveChanges();
            }
        }

        /// <summary>
        /// Освобождает ресурсы контекста. Не сбрасывает Singleton-ссылку,
        /// чтобы другие компоненты могли продолжать использовать контекст.
        /// </summary>
        public override void Dispose()
        {
            // Не обнуляем _instance, так как это нарушит паттерн Singleton.
            base.Dispose();
        }

        /// <summary>
        /// Настраивает провайдер базы данных в зависимости от флага <see cref="_forceInMemory"/>.
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_forceInMemory)
            {
                optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            }
            else
            {
                optionsBuilder.UseSqlServer(@"Server=172.16.1.101,33678;Database=levchenko;User Id=Levchenko;Password=MNroqW(;TrustServerCertificate=True;Trusted_Connection=False;");
            }
        }

        /// <summary>
        /// Настраивает связи между сущностями (ограничения внешних ключей).
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Подписка -> Пользователь
            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.User)
                .WithMany(u => u.Subscriptions)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Подписка -> Канал
            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.Channel)
                .WithMany(c => c.Subscriptions)
                .HasForeignKey(s => s.StreamChannelId)
                .OnDelete(DeleteBehavior.NoAction);

            // Донат -> Пользователь
            modelBuilder.Entity<Donation>()
                .HasOne(d => d.User)
                .WithMany(u => u.Donations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}