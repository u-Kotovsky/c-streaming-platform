using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StreamingPlatformCore.Entities;

/// <summary>
/// Сообщение чата, отправленное пользователем во время трансляции.
/// </summary>
public class ChatMessage
{
    /// <summary>
    /// Уникальный идентификатор сообщения.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Идентификатор пользователя, отправившего сообщение.
    /// </summary>
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }

    /// <summary>
    /// Пользователь, отправивший сообщение.
    /// </summary>
    public virtual User User { get; set; }

    /// <summary>
    /// Идентификатор трансляции, в которой было отправлено сообщение.
    /// </summary>
    [ForeignKey(nameof(LiveStream))]
    public int LiveStreamId { get; set; }

    /// <summary>
    /// Трансляция, к которой относится сообщение.
    /// </summary>
    public virtual LiveStream LiveStream { get; set; }

    /// <summary>
    /// Текст сообщения.
    /// </summary>
    [Required]
    public string Content { get; set; }

    /// <summary>
    /// Дата и время отправки сообщения.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Создаёт новое сообщение чата.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="content">Текст сообщения.</param>
    public ChatMessage(int userId, string content)
    {
        UserId = userId;
        Content = content;
    }
}