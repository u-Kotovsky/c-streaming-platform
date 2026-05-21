using System.ComponentModel.DataAnnotations.Schema;

namespace StreamingPlatformCore.Entities;

public class ChatMessage
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }
    public int AuthorId { get; private set; }
    public string Content { get; private set; }
    public DateTime CreatedAt { get; private set; }

    [ForeignKey(nameof(AuthorId))]
    public User Author { get; private set; }

    public ChatMessage(int authorId, string content)
    {
        CreatedAt = DateTime.UtcNow;
        AuthorId = authorId;
        Content = content;
    }
}