namespace StreamingPlatformCore.Services;

public class StreamChannelService
{
    private ApplicationContext _context;

    public StreamChannelService()
    {
        _context = ApplicationContext.GetInstance();
    }

    public void AddMessage(int streamChannelId, int authorId, string content)
    {
        _context.ChatMessages.Add(new(authorId, content));
        _context.SaveChanges();
    }

    public bool IsSubscribed(int streamChannelId, int userId)
    {
        return _context.Subscriptions.Any(s =>
            s.UserId == userId && s.StreamChannelId == streamChannelId);
    }

    public void AddSubscription(int streamChannelId, int userId)
    {
        if (IsSubscribed(streamChannelId, userId))
        {
            throw new InvalidOperationException("User is already subscribed to this channel");
        }

        _context.Subscriptions.Add(new(userId, streamChannelId));
    }

    public void RemoveSubscription(int streamChannelId, int userId)
    {
        if (!IsSubscribed(streamChannelId, userId))
        {
            throw new InvalidOperationException("User is not subscribed to this channel");
        }

        _context.Subscriptions.Add(new(userId, streamChannelId));
    }
}