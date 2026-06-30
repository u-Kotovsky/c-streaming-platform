using System.Collections.ObjectModel;
using StreamingPlatform.Helpers;
using StreamingPlatformCore;

namespace StreamingPlatform.Models
{
    /// <summary>
    /// ViewModel для списка трансляций.
    /// </summary>
    internal class LiveStreamsViewModel : NotifablePropertyChanged
    {
        private ObservableCollection<LiveStreamModel> _liveStreams;

        /// <summary>
        /// Коллекция трансляций.
        /// </summary>
        public ObservableCollection<LiveStreamModel> LiveStreams
        {
            get => _liveStreams;
            set { _liveStreams = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Загружает все трансляции из базы.
        /// </summary>
        public LiveStreamsViewModel()
        {
            var context = ApplicationContext.GetInstance();
            var streams = context.LiveStreams.ToList();
            LiveStreams = new ObservableCollection<LiveStreamModel>(LiveStreamModel.From(streams));
        }
    }
}