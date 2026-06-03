using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StreamingPlatformCore.Entities;

/// <summary>
/// Represents chat message in the live stream from a user
/// </summary>
public class ChatMessage
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    public virtual User User { get; set; }

    [ForeignKey(nameof(LiveStream))]
    public int LiveStreamId { get; set; }
    public virtual LiveStream LiveStream { get; set; }

    [Required]
    public string Content { get; set; }

    public DateTime CreatedAt { get; set; }
    public ChatMessage(int userId, string content)
    {
        UserId = userId;
        Content = content;
    }
}