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
        public static bool ForceInMemory { get; set; }
        public static bool InsertTestValues { get; set; }

        #region Singleton stuff
        private static ApplicationContext? _instance;
        public static ApplicationContext GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ApplicationContext();
                if (InsertTestValues)
                {
                    _instance.AddTestValues();
                }
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
            SaveChanges();
        }

        private ApplicationContext()
        {
#if DEBUG
            ForceInMemory = true;
            InsertTestValues = true;
            Database.EnsureDeleted();
            Database.EnsureCreated();
#endif
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
            if (ForceInMemory)
            {
                optionsBuilder.UseInMemoryDatabase("db");
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
