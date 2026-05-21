using System.ComponentModel.DataAnnotations.Schema;

namespace StreamingPlatformCore.Entities;

public class Subscription
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public int StreamChannelId { get; private set; }
    public DateTime SubscribedAt { get; private set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; private set; }

    [ForeignKey(nameof(StreamChannelId))]
    public StreamChannel StreamChannel { get; private set; }

    public Subscription(int userId, int streamChannelId)
    {
        SubscribedAt = DateTime.UtcNow;
        UserId = userId;
        StreamChannelId = streamChannelId;
    }
}