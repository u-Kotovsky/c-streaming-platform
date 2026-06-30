using Microsoft.EntityFrameworkCore;
using StreamingPlatformCore;
using StreamingPlatformCore.Entities;
using StreamingPlatformCore.Services;

namespace StreamingPlatformTests
{
    public class Tests
    {
        private ApplicationContext _context;
        private ChannelReportService _reportService;

        [SetUp]
        public void Setup()
        {
            _context = ApplicationContext.GetInstance(true, false);
            _reportService = new ChannelReportService();
            _context.Database.EnsureCreated();

            SendTestData();
        }

        [TearDown]
        public void Destruct()
        {
            ApplicationContext.ResetInstance();
            _context = null;
        }

        private void SendTestData()
        {
            // Создаём тестовые данные
            var user = new User("testuser", "test");
            var category = new Category("Gaming", "gaming");

            _context.Categories.Add(category);
            _context.Users.Add(user);
            _context.SaveChanges();

            var channel = new StreamChannel("Test Channel", "", user.Id)
            { 
                Category = category,
                SubscriberCount = 100
            };

            // Создаём активную подписку
            var activeSubscription = new Subscription(user.Id, channel.Id)
            {
                StartDate = DateTime.Now.AddDays(-15),
                EndDate = DateTime.Now.AddDays(15),
                Price = 9.99m
            };

            // Создаём неактивную подписку
            var inactiveSubscription = new Subscription(user.Id, channel.Id)
            {
                StartDate = DateTime.Now.AddDays(-45),
                EndDate = DateTime.Now.AddDays(-15),
                Price = 9.99m
            };

            // Создаём трансляции
            var stream1 = new LiveStream
            {
                Channel = channel,
                Title = "Test Stream 1",
                StartDate = DateTime.Now.AddDays(-5),
                Duration = TimeSpan.FromHours(2),
                Status = LiveStreamStatus.Ended
            };

            var stream2 = new LiveStream
            {
                Channel = channel,
                Title = "Test Stream 2",
                StartDate = DateTime.Now.AddDays(-3),
                Duration = TimeSpan.FromHours(3),
                Status = LiveStreamStatus.Ended
            };

            // Создаём донаты
            var donation1 = new Donation(user.Id, stream1.Id, 25m)
            {
                User = user,
                LiveStream = stream1,
                DonationDate = DateTime.Now.AddDays(-5)
            };

            var donation2 = new Donation(user.Id, stream2.Id, 50m)
            {
                User = user,
                LiveStream = stream2,
                DonationDate = DateTime.Now.AddDays(-3)
            };

            channel.Subscriptions = new List<Subscription> { activeSubscription, inactiveSubscription };
            channel.LiveStreams = new List<LiveStream> { stream1, stream2 };
            stream1.Donations = new List<Donation> { donation1 };
            stream2.Donations = new List<Donation> { donation2 };

            _context.StreamChannels.Add(channel);
            _context.SaveChanges();
        }

        [Test]
        public void Test_CheckSubscriptionActive()
        {
            // Arrange
            var channel = _context.StreamChannels
                .Include(c => c.Subscriptions)
                .First();
            var activeSub = channel.Subscriptions.First(s => s.EndDate > DateTime.UtcNow);
            var inactiveSub = channel.Subscriptions.First(s => s.EndDate <= DateTime.UtcNow);

            // Act
            bool isActive = activeSub.IsActive;
            bool isInactive = inactiveSub.IsActive;

            // Assert
            Assert.IsTrue(isActive);
            Assert.IsFalse(isInactive);
        }

        [Test]
        public void Test_CalculateChannelRevenue()
        {
            // Arrange
            var channel = _context.StreamChannels
                .Include(c => c.Subscriptions)
                .Include(c => c.LiveStreams)
                    .ThenInclude(s => s.Donations)
                .First();

            // Act
            var revenue = channel.CalculateRevenue();

            // Assert
            // Доход должен быть: 9.99 (активная подписка) + 25.00 + 50.00 (донаты) = 84.99
            Assert.AreEqual(84.99m, revenue);
        }

        [Test]
        public void Test_CalculateAverageStreamDuration()
        {
            // Arrange
            var channel = _context.StreamChannels
                .Include(c => c.LiveStreams)
                .First();

            // Act
            var averageDuration = channel.GetAverageStreamDuration();

            // Assert
            // Средняя длительность: (2 + 3) / 2 = 2.5 часа
            Assert.AreEqual(2.5, averageDuration);
        }

        [Test]
        public void Test_GenerateChannelReport()
        {
            // Arrange
            var channel = _context.StreamChannels.First();

            // Act
            var report = _reportService.GenerateChannelReport(channel.Id);

            // Assert
            Assert.IsNotNull(report);
            Assert.AreEqual("Test Channel", report.ChannelName);
            Assert.AreEqual("testuser", report.AuthorName);
            Assert.AreEqual("Gaming", report.Category);
            Assert.AreEqual(100, report.SubscriberCount);
            Assert.AreEqual(2, report.TotalStreams);
            Assert.AreEqual(2.5, report.AverageDuration);
            Assert.AreEqual(84.99m, report.TotalRevenue);
            Assert.AreEqual(9.99m, report.SubscriptionRevenue);
            Assert.AreEqual(75.00m, report.DonationRevenue);
            Assert.AreEqual(1, report.ActiveSubscriptions);
        }

        [Test]
        public void Test_EmptyChannelReport()
        {
            // Arrange
            var user = new User("emptyuser", "");
            _context.Users.Add(user);
            _context.SaveChanges();

            var category = new Category("Other", "desc");
            _context.Categories.Add(category);
            _context.SaveChanges();

            var emptyChannel = new StreamChannel("Empty Channel", "desc", user.Id)
            {
                Category = category,
                CategoryId = category.Id
            };
            _context.StreamChannels.Add(emptyChannel);
            _context.SaveChanges();

            // Act
            var report = _reportService.GenerateChannelReport(emptyChannel.Id);

            // Assert
            Assert.AreEqual(0, report.TotalStreams);
            Assert.AreEqual(0, report.AverageDuration);
            Assert.AreEqual(0m, report.TotalRevenue);
        }
    }
}