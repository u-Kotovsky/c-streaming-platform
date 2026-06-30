using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StreamingPlatformCore.Entities;

/// <summary>
/// Статус трансляции.
/// </summary>
public enum LiveStreamStatus
{
    /// <summary>
    /// Запланирована.
    /// </summary>
    Scheduled,
    /// <summary>
    /// Идёт в прямом эфире.
    /// </summary>
    Live,
    /// <summary>
    /// Завершена.
    /// </summary>
    Ended,
    /// <summary>
    /// Отменена.
    /// </summary>
    Cancelled
}

/// <summary>
/// Трансляция (стрим) на канале.
/// </summary>
public class LiveStream
{
    /// <summary>
    /// Уникальный идентификатор трансляции.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Идентификатор канала, на котором проходит трансляция.
    /// </summary>
    [ForeignKey(nameof(Channel))]
    public int StreamChannelId { get; set; }

    /// <summary>
    /// Канал, владеющий трансляцией.
    /// </summary>
    public virtual StreamChannel Channel { get; set; }

    /// <summary>
    /// Название трансляции.
    /// </summary>
    [Required]
    public string Title { get; set; }

    /// <summary>
    /// Дата и время начала трансляции.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Продолжительность трансляции.
    /// </summary>
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// Текущий статус трансляции.
    /// </summary>
    public LiveStreamStatus Status { get; set; }

    /// <summary>
    /// Количество зрителей (пиковое или текущее).
    /// </summary>
    public uint ViewerCount { get; set; }

    /// <summary>
    /// Список донатов, полученных во время этой трансляции.
    /// </summary>
    public virtual ICollection<Donation> Donations { get; set; }

    /// <summary>
    /// Сообщения чата, отправленные во время трансляции.
    /// </summary>
    public virtual ICollection<ChatMessage> ChatMessages { get; set; }
}