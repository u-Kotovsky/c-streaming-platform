using Microsoft.EntityFrameworkCore;
using StreamingPlatformCore.Entities;

namespace StreamingPlatformCore.Services;

/// <summary>
/// Сервис для формирования отчёта по каналу (статистика, доходы, подписки).
/// </summary>
public class ChannelReportService
{
    private readonly ApplicationContext _context;

    /// <summary>
    /// Инициализирует сервис и получает контекст базы данных.
    /// </summary>
    public ChannelReportService()
    {
        _context = ApplicationContext.GetInstance();
    }

    /// <summary>
    /// Генерирует полный отчёт по указанному каналу.
    /// </summary>
    /// <param name="channelId">Идентификатор канала.</param>
    /// <returns>Объект <see cref="ChannelReport"/> с агрегированной статистикой.</returns>
    /// <exception cref="ArgumentException">Выбрасывается, если канал не найден.</exception>
    public ChannelReport GenerateChannelReport(int channelId)
    {
        var channel = _context.StreamChannels
            .Include(c => c.Author)
            .Include(c => c.Category)
            .Include(c => c.Subscriptions)
            .Include(c => c.LiveStreams)
                .ThenInclude(s => s.Donations)
            .FirstOrDefault(c => c.Id == channelId);

        if (channel == null)
        {
            throw new ArgumentException("Channel not found");
        }

        var report = new ChannelReport
        {
            ChannelName = channel.Name,
            AuthorName = channel.Author?.Username ?? "Unknown",
            Category = channel.Category?.Name ?? "Uncategorized",
            SubscriberCount = channel.SubscriberCount,
            TotalStreams = channel.LiveStreams?.Count ?? 0,
            AverageDuration = channel.GetAverageStreamDuration(),
            TotalRevenue = channel.CalculateRevenue(),
            SubscriptionRevenue = channel.Subscriptions?
                .Where(s => s.IsActive)
                .Sum(s => s.Price) ?? 0,
            DonationRevenue = channel.LiveStreams?
                .SelectMany(s => s.Donations ?? Enumerable.Empty<Donation>())
                .Sum(d => d.Amount) ?? 0,
            ActiveSubscriptions = channel.Subscriptions?
                .Count(s => s.IsActive) ?? 0
        };

        return report;
    }
}