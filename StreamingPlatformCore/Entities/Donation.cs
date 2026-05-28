using System.ComponentModel.DataAnnotations.Schema;

namespace StreamingPlatformCore.Entities;

/// <summary>
/// Represents donation to the live stream
/// </summary>
public class Donation
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    public virtual User User { get; set; }


    [ForeignKey(nameof(LiveStream))]
    public int LiveStreamId { get; set; }
    public virtual LiveStream LiveStream { get; set; }

    public decimal Amount { get; set; }

    public string Message { get; set; }

    public DateTime DonationDate { get; set; }
}