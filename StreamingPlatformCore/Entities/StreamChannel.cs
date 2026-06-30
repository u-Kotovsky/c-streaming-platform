using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StreamingPlatformCore.Entities;

/// <summary>
/// Канал стриминговой платформы, который может проводить трансляции, получать подписки и донаты.
/// </summary>
public class StreamChannel
{
    /// <summary>
    /// Уникальный идентификатор канала.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Название канала.
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Заголовок канала (псевдоним для Name).
    /// </summary>
    public string Title { get => Name; set { Name = value; } }

    /// <summary>
    /// Описание канала.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор автора (владельца) канала.
    /// </summary>
    [ForeignKey(nameof(Author))]
    public int AuthorId { get; set; }

    /// <summary>
    /// Автор канала (пользователь-владелец).
    /// </summary>
    public virtual User Author { get; set; }

    /// <summary>
    /// Идентификатор категории канала.
    /// </summary>
    [ForeignKey(nameof(Category))]
    public int CategoryId { get; set; }

    /// <summary>
    /// Категория, к которой относится канал.
    /// </summary>
    public virtual Category Category { get; set; }

    /// <summary>
    /// Текущее количество подписчиков канала.
    /// </summary>
    public uint SubscriberCount { get; set; }

    /// <summary>
    /// Список трансляций, проведённых на канале.
    /// </summary>
    public virtual ICollection<LiveStream> LiveStreams { get; set; }

    /// <summary>
    /// Список подписок на канал.
    /// </summary>
    public virtual ICollection<Subscription> Subscriptions { get; set; }

    /// <summary>
    /// Создаёт новый канал.
    /// </summary>
    /// <param name="name">Название канала.</param>
    /// <param name="description">Описание канала.</param>
    /// <param name="authorId">Идентификатор владельца.</param>
    public StreamChannel(string name, string description, int authorId)
    {
        Name = name;
        Description = description;
        AuthorId = authorId;
    }

    /// <summary>
    /// Рассчитывает общий доход канала от активных подписок и всех донатов.
    /// </summary>
    /// <returns>Сумма дохода.</returns>
    public decimal CalculateRevenue()
    {
        decimal subscriptionsRevenue = 0;
        decimal donationsRevenue = 0;

        if (Subscriptions != null)
        {
            foreach (var sub in Subscriptions)
            {
                if (sub.IsActive)
                {
                    subscriptionsRevenue += sub.Price;
                }
            }
        }

        if (LiveStreams != null)
        {
            foreach (var liveStream in LiveStreams)
            {
                foreach (var donation in liveStream.Donations)
                {
                    donationsRevenue += donation.Amount;
                }
            }
        }

        return subscriptionsRevenue + donationsRevenue;
    }

    /// <summary>
    /// Вычисляет среднюю длительность трансляций канала в часах.
    /// </summary>
    /// <returns>Средняя длительность (0, если трансляций нет).</returns>
    public double GetAverageStreamDuration()
    {
        if (LiveStreams == null || LiveStreams.Count == 0)
            return 0;

        double totalDuration = 0;
        foreach (var stream in LiveStreams)
        {
            totalDuration += stream.Duration.TotalHours;
        }

        return totalDuration / LiveStreams.Count;
    }
}