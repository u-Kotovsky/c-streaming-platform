using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StreamingPlatformCore.Entities;

/// <summary>
/// Represents current status of live stream.
/// </summary>
public enum LiveStreamStatus
{
    Scheduled,
    Live,
    Ended,
    Cancelled
}

/// <summary>
/// Represents a live stream
/// </summary>
public class LiveStream
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey(nameof(Channel))]
    public int StreamChannelId { get; set; }
    public virtual StreamChannel Channel { get; set; }

    [Required]
    public string Title { get; set; }

    public DateTime StartDate { get; set; }

    public TimeSpan Duration { get; set; }

    public LiveStreamStatus Status { get; set; }

    public uint ViewerCount { get; set; }

    public virtual ICollection<Donation> Donations { get; set; }
    public virtual ICollection<ChatMessage> ChatMessages { get; set; }
}