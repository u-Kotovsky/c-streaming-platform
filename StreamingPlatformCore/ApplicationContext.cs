using Microsoft.EntityFrameworkCore;
using StreamingPlatformCore.Entities;

namespace StreamingPlatformCore
{
    /// <summary>
    /// Main application context to control database connection
    /// </summary>
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<StreamChannel> StreamChannels { get; set; }
        public DbSet<LiveStream> LiveStreams { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Donation> Donates { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Category> Categories { get; set; }

        #region Singleton stuff
        private static ApplicationContext? _instance;
        public static ApplicationContext GetInstance(bool forceInMemory = false, bool insertTestValues = false)
        {
            if (_instance == null)
            {
                _instance = new ApplicationContext(forceInMemory, insertTestValues);
            }
            return _instance;
        }
        #endregion

        public override void Dispose()
        {
            _instance = null;
            base.Dispose();
        }

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

        private bool forceInMemory = false;

        private ApplicationContext(bool forceInMemory = false, bool insertTestValues = false)
        {
            this.forceInMemory = forceInMemory;

#if DEBUG
            Database.EnsureDeleted();
            Database.EnsureCreated();
#endif

            if (insertTestValues)
            {
                AddTestValues();
            }
        }

        /// <summary>
        /// Properly destroy instance
        /// </summary>
        ~ApplicationContext()
        {
            _instance = null;
            try
            {
#if DEBUG
                //Database.EnsureDeleted();
#endif
            }
            catch (Exception)
            {
                // ignore
            }
            finally
            {

                Dispose();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (forceInMemory)
            {
                optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            }
            else
            {
                optionsBuilder.UseSqlServer(@"Server=172.16.1.101,33678;Database=levchenko;User Id=Levchenko;Password=MNroqW(;TrustServerCertificate=True;Trusted_Connection=False;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.User)
                .WithMany(u => u.Subscriptions)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.Channel)
                .WithMany(c => c.Subscriptions)
                .HasForeignKey(s => s.StreamChannelId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Donation>()
                .HasOne(d => d.User)
                .WithMany(u => u.Donations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
