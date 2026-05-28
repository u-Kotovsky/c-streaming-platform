namespace StreamingPlatformCore.Entities;

/// <summary>
/// Represents report statistics of the stream channel
/// </summary>
public class ChannelReport
{
    public string ChannelName { get; set; }
    public string AuthorName { get; set; }
    public string Category { get; set; }
    public uint SubscriberCount { get; set; }
    public int TotalStreams { get; set; }
    public double AverageDuration { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal SubscriptionRevenue { get; set; }
    public decimal DonationRevenue { get; set; }
    public int ActiveSubscriptions { get; set; }
}