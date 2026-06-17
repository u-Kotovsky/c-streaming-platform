using System.Windows;
using StreamingPlatformCore.Entities;

namespace StreamingPlatform.Models
{
    internal class StreamChannelModel : StreamChannel, InteractableModel
    {
        public StreamChannelModel(string name, string description, int authorId) : base(name, description, authorId)
        {
            OnInteract += (channel) => MessageBox.Show($"{channel.Name}");
        }

        public event Action<StreamChannel> OnInteract = delegate { };

        public static StreamChannelModel From(StreamChannel channel)
        {
            var obj = new StreamChannelModel(channel.Name, channel.Description, channel.AuthorId);

            return obj;
        }

        public static List<StreamChannelModel> From(List<StreamChannel> channels)
        {
            var list = new List<StreamChannelModel>();

            foreach (var channel in channels)
            {
                list.Add(From(channel));
            }

            return list;
        }

        public void Interact()
        {
            OnInteract?.Invoke(this);
        }
    }
}