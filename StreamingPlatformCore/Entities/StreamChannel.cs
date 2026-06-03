using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace StreamingPlatformCore.Entities;

/// <summary>
/// Represents channel that can go live, receive donations, subscriptions n stuff.
/// </summary>
public class StreamChannel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }
    public string Description { get; set; }

    [ForeignKey(nameof(Author))]
    public int AuthorId { get; set; }
    public virtual User Author { get; set; }

    [ForeignKey(nameof(Category))]
    public int CategoryId { get; set; }
    public virtual Category Category { get; set; }

    public uint SubscriberCount { get; set; }

    public virtual ICollection<LiveStream> LiveStreams { get; set; }
    public virtual ICollection<Subscription> Subscriptions { get; set; }

    public StreamChannel(string name, string description, int authorId)
    {
        Name = name;
        Description = description;
        AuthorId = authorId;
    }

    /// <summary>
    /// Calculate revenue of the channel
    /// </summary>
    /// <returns></returns>
    public decimal CalculateRevenue()
    {
        decimal subscriptionsRevenue = 0;
        decimal donationsRevenue = 0;

        if (Subscriptions != null)
        {
            foreach (var sub in Subscriptions)
            {
                if (sub.IsActive)
                {
                    subscriptionsRevenue += sub.Price;
                }
            }
        }

        if (LiveStreams != null)
        {
            foreach (var liveStream in LiveStreams)
            {
                foreach (var donation in liveStream.Donations)
                {
                    donationsRevenue += donation.Amount;
                }
            }
        }

        return subscriptionsRevenue + donationsRevenue;
    }

    /// <summary>
    /// Calculate average duration of live stream.
    /// </summary>
    /// <returns></returns>
    public double GetAverageStreamDuration()
    {
        if (LiveStreams == null || LiveStreams.Count == 0)
            return 0;

        double totalDuration = 0;
        foreach (var stream in LiveStreams)
        {
            totalDuration += stream.Duration.TotalHours;
        }

        return totalDuration / LiveStreams.Count;
    }
}