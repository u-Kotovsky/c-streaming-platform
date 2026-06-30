using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StreamingPlatformCore.Entities;

/// <summary>
/// Платная подписка пользователя на канал.
/// </summary>
public class Subscription
{
    /// <summary>
    /// Уникальный идентификатор подписки.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Идентификатор пользователя, оформившего подписку.
    /// </summary>
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }

    /// <summary>
    /// Пользователь, оформивший подписку.
    /// </summary>
    public virtual User User { get; set; }

    /// <summary>
    /// Идентификатор канала, на который оформлена подписка.
    /// </summary>
    [ForeignKey(nameof(Channel))]
    public int StreamChannelId { get; set; }

    /// <summary>
    /// Канал, на который оформлена подписка.
    /// </summary>
    public virtual StreamChannel Channel { get; set; }

    /// <summary>
    /// Дата начала действия подписки.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Дата окончания действия подписки.
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Стоимость подписки.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Признак активности подписки (текущая дата находится между StartDate и EndDate).
    /// </summary>
    public bool IsActive => DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate;

    /// <summary>
    /// Создаёт новую подписку.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="streamChannelId">Идентификатор канала.</param>
    public Subscription(int userId, int streamChannelId)
    {
        UserId = userId;
        StreamChannelId = streamChannelId;
    }
}