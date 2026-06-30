using Microsoft.EntityFrameworkCore;
using StreamingPlatformCore.Entities;

namespace StreamingPlatformCore.Services;

/// <summary>
/// Сервис для управления каналами, подписками и сообщениями.
/// </summary>
public class StreamChannelService
{
    private readonly ApplicationContext _context;

    /// <summary>
    /// Инициализирует сервис и получает контекст базы данных.
    /// </summary>
    public StreamChannelService()
    {
        _context = ApplicationContext.GetInstance();
    }

    /// <summary>
    /// Добавляет новое сообщение в чат указанного канала.
    /// </summary>
    /// <param name="streamChannelId">Идентификатор канала (в рамках которого трансляция).</param>
    /// <param name="authorId">Идентификатор автора сообщения.</param>
    /// <param name="content">Текст сообщения.</param>
    public void AddMessage(int streamChannelId, int authorId, string content)
    {
        _context.ChatMessages.Add(new ChatMessage(authorId, content));
        _context.SaveChanges();
    }

    /// <summary>
    /// Проверяет, подписан ли указанный пользователь на канал.
    /// </summary>
    /// <param name="streamChannelId">Идентификатор канала.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>true, если активная подписка существует.</returns>
    public bool IsSubscribed(int streamChannelId, int userId)
    {
        return _context.Subscriptions.Any(s =>
            s.UserId == userId && s.StreamChannelId == streamChannelId);
    }

    /// <summary>
    /// Оформляет новую подписку пользователя на канал.
    /// </summary>
    /// <param name="streamChannelId">Идентификатор канала.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="price">Стоимость подписки.</param>
    /// <param name="durationDays">Длительность подписки в днях.</param>
    /// <exception cref="InvalidOperationException">Если пользователь уже подписан.</exception>
    public void AddSubscription(int streamChannelId, int userId, decimal price, int durationDays)
    {
        if (IsSubscribed(streamChannelId, userId))
        {
            throw new InvalidOperationException("User is already subscribed to this channel");
        }

        var subscription = new Subscription(userId, streamChannelId)
        {
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(durationDays),
            Price = price
        };

        _context.Subscriptions.Add(subscription);

        // Увеличиваем счётчик подписчиков канала
        var channel = _context.StreamChannels.Find(streamChannelId);
        if (channel != null)
        {
            channel.SubscriberCount++;
        }

        _context.SaveChanges();
    }

    /// <summary>
    /// Удаляет подписку пользователя на канал.
    /// </summary>
    /// <param name="streamChannelId">Идентификатор канала.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <exception cref="InvalidOperationException">Если подписка не найдена.</exception>
    public void RemoveSubscription(int streamChannelId, int userId)
    {
        var subscription = _context.Subscriptions
            .FirstOrDefault(s => s.UserId == userId && s.StreamChannelId == streamChannelId);

        if (subscription == null)
        {
            throw new InvalidOperationException("Subscription not found");
        }

        _context.Subscriptions.Remove(subscription);

        // Уменьшаем счётчик подписчиков канала
        var channel = _context.StreamChannels.Find(streamChannelId);
        if (channel != null && channel.SubscriberCount > 0)
        {
            channel.SubscriberCount--;
        }

        _context.SaveChanges();
    }
}