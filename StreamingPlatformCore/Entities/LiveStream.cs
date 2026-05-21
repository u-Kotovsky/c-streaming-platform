using System.ComponentModel.DataAnnotations.Schema;

namespace StreamingPlatformCore.Entities;

public class LiveStream
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }
    public int StreamChannelId { get; private set; }
    public string Name { get; private set; }
    public DateTime StartedAt { get; private set; }
    public TimeSpan Duration { get; private set; }
    public int Status { get; private set; }

    [ForeignKey(nameof(StreamChannelId))]
    public StreamChannel StreamChannel { get; private set; }

    public LiveStream(int streamChannelId, string name, DateTime startedAt, TimeSpan duration, int status)
    {
        StreamChannelId = streamChannelId;
        Name = name;
        StartedAt = startedAt;
        Duration = duration;
        Status = status;
    }
}