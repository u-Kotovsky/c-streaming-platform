using System.Windows;
using System.Windows.Input;
using StreamingPlatform.Helpers;
using StreamingPlatformCore;

namespace StreamingPlatform.Models
{
    public class EditChannelViewModel : NotifablePropertyChanged
    {
        private readonly int _channelId;
        private readonly ApplicationContext _context;

        public string Name { get; set; }
        public string Description { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public EditChannelViewModel(int channelId)
        {
            _channelId = channelId;
            _context = ApplicationContext.GetInstance();
            var channel = _context.StreamChannels.Find(channelId);
            if (channel != null)
            {
                Name = channel.Name;
                Description = channel.Description;
            }
            SaveCommand = new RelayCommand(_ => Save());
            CancelCommand = new RelayCommand(_ => CloseWindow(false));
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Name)) return;
            var channel = _context.StreamChannels.Find(_channelId);
            if (channel != null)
            {
                channel.Name = Name;
                channel.Description = Description;
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