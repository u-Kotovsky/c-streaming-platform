using System.Collections.ObjectModel;
using StreamingPlatform.Helpers;
using StreamingPlatformCore;

namespace StreamingPlatform.Models
{
    /// <summary>
    /// ViewModel для списка каналов.
    /// </summary>
    internal class ChannelsViewModel : NotifablePropertyChanged
    {
        private ObservableCollection<StreamChannelModel> _channels;

        /// <summary>
        /// Коллекция каналов для отображения.
        /// </summary>
        public ObservableCollection<StreamChannelModel> Channels
        {
            get => _channels;
            set { _channels = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Загружает все каналы из базы.
        /// </summary>
        public ChannelsViewModel()
        {
            var context = ApplicationContext.GetInstance();
            var channels = context.StreamChannels.ToList();
            Channels = new ObservableCollection<StreamChannelModel>(StreamChannelModel.From(channels));
        }
    }
}