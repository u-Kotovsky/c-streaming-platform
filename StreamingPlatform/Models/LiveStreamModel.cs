using StreamingPlatformCore.Entities;

namespace StreamingPlatform.Models
{
    internal class LiveStreamModel : LiveStream, InteractableModel
    {

        public event Action<LiveStream> OnInteract = delegate { };

        public static LiveStreamModel From(LiveStream channel)
        {
            var obj = new LiveStreamModel
            {
                Id = channel.Id,
                StreamChannelId = channel.StreamChannelId,
                Title = channel.Title,
                StartDate = channel.StartDate,
                Duration = channel.Duration,
                Status = channel.Status,
                ViewerCount = channel.ViewerCount,
                Donations = channel.Donations,
                ChatMessages = channel.ChatMessages,
            };

            return obj;
        }

        public static List<LiveStreamModel> From(List<LiveStream> channels)
        {
            var list = new List<LiveStreamModel>();

            foreach (var channel in channels)
            {
                list.Add(From(channel));
            }

            return list;
        }

        public void InteractWithModel()
        {
            OnInteract?.Invoke(this);
        }
    }
}
