using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StreamingPlatformCore.Entities;

/// <summary>
/// Пользователь системы. Может владеть каналами, отправлять сообщения, делать донаты и оформлять подписки.
/// </summary>
public class User
{
    /// <summary>
    /// Уникальный идентификатор пользователя.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Имя пользователя (логин).
    /// </summary>
    [Required]
    public string Username { get; set; }

    /// <summary>
    /// Пароль (в учебном проекте хранится в открытом виде).
    /// </summary>
    [Required]
    public string Password { get; set; }

    /// <summary>
    /// Дата регистрации пользователя.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Подписки, оформленные пользователем.
    /// </summary>
    public virtual ICollection<Subscription> Subscriptions { get; set; }

    /// <summary>
    /// Донаты, сделанные пользователем.
    /// </summary>
    public virtual ICollection<Donation> Donations { get; set; }

    /// <summary>
    /// Сообщения чата, отправленные пользователем.
    /// </summary>
    public virtual ICollection<ChatMessage> ChatMessages { get; set; }

    /// <summary>
    /// Каналы, которыми владеет пользователь.
    /// </summary>
    public virtual ICollection<StreamChannel> OwnedChannels { get; set; }

    /// <summary>
    /// Создаёт нового пользователя.
    /// </summary>
    /// <param name="username">Имя пользователя.</param>
    /// <param name="password">Пароль.</param>
    public User(string username, string password)
    {
        Username = username;
        Password = password;
    }
}