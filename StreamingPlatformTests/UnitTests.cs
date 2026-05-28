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
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "TestStreamingDB")
                .Options;

            _context = ApplicationContext.GetInstance();
            _reportService = new ChannelReportService();

            SeedTestData();
        }

        [TearDown]
        public void Destruct()
        {
            _context.Dispose();
        }

        private void SeedTestData()
        {
            // Создаём тестовые данные
            var user = new User { Username = "testuser", Password = "test" };
            var category = new Category { Name = "Gaming" };
            var channel = new StreamChannel
            {
                Name = "Test Channel",
                Author = user,
                Category = category,
                SubscriberCount = 100
            };

            // Создаём активную подписку
            var activeSubscription = new Subscription
            {
                User = user,
                Channel = channel,
                StartDate = DateTime.Now.AddDays(-15),
                EndDate = DateTime.Now.AddDays(15),
                Price = 9.99m
            };

            // Создаём неактивную подписку
            var inactiveSubscription = new Subscription
            {
                User = user,
                Channel = channel,
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
            var donation1 = new Donation
            {
                User = user,
                LiveStream = stream1,
                Amount = 25.00m,
                DonationDate = DateTime.Now.AddDays(-5)
            };

            var donation2 = new Donation
            {
                User = user,
                LiveStream = stream2,
                Amount = 50.00m,
                DonationDate = DateTime.Now.AddDays(-3)
            };

            channel.Subscriptions = new List<Subscription> { activeSubscription, inactiveSubscription };
            channel.LiveStreams = new List<LiveStream> { stream1, stream2 };
            stream1.Donations = new List<Donation> { donation1 };
            stream2.Donations = new List<Donation> { donation2 };

            _context.Users.Add(user);
            _context.Categories.Add(category);
            _context.StreamChannels.Add(channel);
            _context.SaveChanges();
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
            var emptyChannel = new StreamChannel
            {
                Name = "Empty Channel",
                Author = new User { Username = "emptyuser" },
                Category = new Category { Name = "Other" }
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

        // TODO: calc stream channel income
        // TODO: check subscription
        // todo: check average duration (of live stream?)
        // todo: report generation
    }
}