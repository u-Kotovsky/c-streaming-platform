using System.ComponentModel.DataAnnotations.Schema;

namespace StreamingPlatformCore.Entities;

public class Donate
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }
    public int AuthorId { get; private set; }
    public int StreamChannelId { get; private set; }
    public double Amount { get; private set; }
    public DateTime DonatedAt { get; private set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public User Author { get; private set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public StreamChannel StreamChannel { get; private set; }

    public Donate(int authorId, int streamChannelId, double amount)
    {
        DonatedAt = DateTime.UtcNow;
        AuthorId = authorId;
        StreamChannelId = streamChannelId;
        Amount = amount;
    }
}