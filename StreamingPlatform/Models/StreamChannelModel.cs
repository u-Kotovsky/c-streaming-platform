using StreamingPlatformCore.Entities;

namespace StreamingPlatform.Models
{
    internal class StreamChannelModel : StreamChannel, InteractableModel
    {
        public StreamChannelModel(string name, string description, int authorId) : base(name, description, authorId)
        {
        }

        public event Action<StreamChannel> OnInteract = delegate { };

        public static StreamChannelModel From(StreamChannel channel)
        {
            return new StreamChannelModel(channel.Name, channel.Description, channel.AuthorId)
            {
                Id = channel.Id,
                CategoryId = channel.CategoryId,
                SubscriberCount = channel.SubscriberCount,
                Author = channel.Author,
                Category = channel.Category,
                Subscriptions = channel.Subscriptions,
                LiveStreams = channel.LiveStreams
            };
        }

        public static List<StreamChannelModel> From(List<StreamChannel> channels)
        {
            return channels.Select(From).ToList();
        }

        public void Interact()
        {
            OnInteract?.Invoke(this);
        }
    }
}