namespace StreamingPlatformCore.Entities;

/// <summary>
/// Отчёт по каналу, содержащий агрегированную статистику.
/// </summary>
public class ChannelReport
{
    /// <summary>
    /// Название канала.
    /// </summary>
    public string ChannelName { get; set; }

    /// <summary>
    /// Имя автора (владельца) канала.
    /// </summary>
    public string AuthorName { get; set; }

    /// <summary>
    /// Категория канала.
    /// </summary>
    public string Category { get; set; }

    /// <summary>
    /// Текущее количество подписчиков.
    /// </summary>
    public uint SubscriberCount { get; set; }

    /// <summary>
    /// Общее количество проведённых трансляций.
    /// </summary>
    public int TotalStreams { get; set; }

    /// <summary>
    /// Средняя продолжительность трансляции в часах.
    /// </summary>
    public double AverageDuration { get; set; }

    /// <summary>
    /// Общий доход канала (подписки + донаты).
    /// </summary>
    public decimal TotalRevenue { get; set; }

    /// <summary>
    /// Доход, полученный только от активных подписок.
    /// </summary>
    public decimal SubscriptionRevenue { get; set; }

    /// <summary>
    /// Доход, полученный от донатов.
    /// </summary>
    public decimal DonationRevenue { get; set; }

    /// <summary>
    /// Количество активных подписок на момент формирования отчёта.
    /// </summary>
    public int ActiveSubscriptions { get; set; }
}