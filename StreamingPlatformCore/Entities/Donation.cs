using System.ComponentModel.DataAnnotations.Schema;

namespace StreamingPlatformCore.Entities;

/// <summary>
/// Донат (пожертвование) от пользователя во время трансляции.
/// </summary>
public class Donation
{
    /// <summary>
    /// Уникальный идентификатор доната.
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Идентификатор пользователя, сделавшего донат.
    /// </summary>
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }

    /// <summary>
    /// Пользователь, сделавший донат.
    /// </summary>
    public virtual User User { get; set; }

    /// <summary>
    /// Идентификатор трансляции, во время которой был сделан донат.
    /// </summary>
    [ForeignKey(nameof(LiveStream))]
    public int LiveStreamId { get; set; }

    /// <summary>
    /// Трансляция, к которой относится донат.
    /// </summary>
    public virtual LiveStream LiveStream { get; set; }

    /// <summary>
    /// Сумма доната.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Сообщение, прикреплённое к донату (необязательное).
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Дата и время совершения доната.
    /// </summary>
    public DateTime DonationDate { get; set; }

    /// <summary>
    /// Создаёт новый донат.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="liveStreamId">Идентификатор трансляции.</param>
    /// <param name="amount">Сумма доната.</param>
    public Donation(int userId, int liveStreamId, decimal amount)
    {
        DonationDate = DateTime.UtcNow;
        UserId = userId;
        LiveStreamId = liveStreamId;
        Amount = amount;
    }
}