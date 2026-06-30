using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using StreamingPlatform.Helpers;
using StreamingPlatformCore;
using StreamingPlatformCore.Entities;
using StreamingPlatformCore.Services;

namespace StreamingPlatform.Models
{
    /// <summary>
    /// ViewModel для главной страницы (списки каналов и стримов, навигация).
    /// </summary>
    internal class MasterViewModel : NotifablePropertyChanged
    {
        private readonly ApplicationContext _context;

        private ObservableCollection<StreamChannelModel> _streamChannels;
        public ObservableCollection<StreamChannelModel> StreamChannels
        {
            get => _streamChannels;
            set { _streamChannels = value; OnPropertyChanged(); }
        }

        private ObservableCollection<LiveStreamModel> _liveStreams;
        public ObservableCollection<LiveStreamModel> LiveStreams
        {
            get => _liveStreams;
            set { _liveStreams = value; OnPropertyChanged(); }
        }

        public ICommand AddChannelCommand { get; }
        public ICommand AddStreamCommand { get; }
        public ICommand DeleteChannelCommand { get; }
        public ICommand DeleteStreamCommand { get; }

        /// <summary>
        /// Инициализирует MasterViewModel и загружает данные.
        /// </summary>
        public MasterViewModel()
        {
            _context = ApplicationContext.GetInstance();
            PopulateLists();

            AddChannelCommand = new RelayCommand(_ => AddChannel());
            AddStreamCommand = new RelayCommand(_ => AddStream());
            DeleteChannelCommand = new RelayCommand(obj => DeleteChannel((int)obj));
            DeleteStreamCommand = new RelayCommand(obj => DeleteStream((int)obj));
            GoBackCommand = new RelayCommand(_ => MainWindow.GetInstance().GoBack());
        }

        public void RefreshLists()
        {
            PopulateLists();
        }

        private void PopulateLists()
        {
            var channels = _context.StreamChannels.ToList();
            var streams = _context.LiveStreams.ToList();
            StreamChannels = new ObservableCollection<StreamChannelModel>(StreamChannelModel.From(channels));
            LiveStreams = new ObservableCollection<LiveStreamModel>(LiveStreamModel.From(streams));
        }

        private void AddChannel()
        {
            // Простейшее добавление через ввод имени
            string name = Microsoft.VisualBasic.Interaction.InputBox("Введите название канала:", "Новый канал", "Канал");
            if (!string.IsNullOrWhiteSpace(name))
            {
                var userService = new UserService();
                var channel = new StreamChannel(name, "", userService.CurrentUser.Id);
                _context.StreamChannels.Add(channel);
                _context.SaveChanges();
                PopulateLists();
            }
        }

        private void DeleteChannel(int id)
        {
            var channel = _context.StreamChannels.Find(id);
            if (channel != null)
            {
                _context.StreamChannels.Remove(channel);
                _context.SaveChanges();
                PopulateLists();
            }
        }

        private void AddStream()
        {
            string title = Microsoft.VisualBasic.Interaction.InputBox("Введите название трансляции:", "Новая трансляция", "Стрим");
            if (!string.IsNullOrWhiteSpace(title))
            {
                if (StreamChannels.Count == 0)
                {
                    MessageBox.Show("Сначала создайте канал.");
                    return;
                }
                var stream = new LiveStream
                {
                    Title = title,
                    StreamChannelId = StreamChannels[0].Id, // берём первый канал для примера
                    StartDate = DateTime.Now,
                    Duration = TimeSpan.FromHours(1),
                    Status = LiveStreamStatus.Scheduled
                };
                _context.LiveStreams.Add(stream);
                _context.SaveChanges();
                PopulateLists();
            }
        }

        private void DeleteStream(int id)
        {
            var stream = _context.LiveStreams.Find(id);
            if (stream != null)
            {
                _context.LiveStreams.Remove(stream);
                _context.SaveChanges();
                PopulateLists();
            }
        }

        public ICommand GoBackCommand { get; }
        public bool CanGoBack => MainWindow.GetInstance().CanGoBack;
        public void RefreshCanGoBack()
        {
            OnPropertyChanged(nameof(CanGoBack));
        }
    }
}