using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using StreamingPlatform.Helpers;
using StreamingPlatformCore;
using StreamingPlatformCore.Entities;

namespace StreamingPlatform.Models
{
    public class EditStreamViewModel : NotifablePropertyChanged
    {
        private readonly int _streamId;
        private readonly ApplicationContext _context;

        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public TimeSpan Duration { get; set; }
        public LiveStreamStatus Status { get; set; }
        public int? SelectedChannelId { get; set; }
        public ObservableCollection<StreamChannel> Channels { get; set; }

        public List<LiveStreamStatus> Statuses { get; } = Enum.GetValues(typeof(LiveStreamStatus)).Cast<LiveStreamStatus>().ToList();

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public EditStreamViewModel(int streamId)
        {
            _streamId = streamId;
            _context = ApplicationContext.GetInstance();

            var stream = _context.LiveStreams.Find(streamId);
            if (stream != null)
            {
                Title = stream.Title;
                StartDate = stream.StartDate;
                Duration = stream.Duration;
                Status = stream.Status;
                SelectedChannelId = stream.StreamChannelId;
            }

            Channels = new ObservableCollection<StreamChannel>(_context.StreamChannels.ToList());
            SaveCommand = new RelayCommand(_ => Save());
            CancelCommand = new RelayCommand(_ => CloseWindow(false));
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Title) || SelectedChannelId == null) return;
            var stream = _context.LiveStreams.Find(_streamId);
            if (stream != null)
            {
                stream.Title = Title;
                stream.StartDate = StartDate;
                stream.Duration = Duration;
                stream.Status = Status;
                stream.StreamChannelId = SelectedChannelId.Value;
                _context.SaveChanges();
                CloseWindow(true);
            }
        }

        private void CloseWindow(bool result)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.DialogResult = result;
                    window.Close();
                    break;
                }
            }
        }
    }
}