using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using StreamingPlatform.Helpers;
using StreamingPlatformCore;
using StreamingPlatformCore.Entities;

namespace StreamingPlatform.Models
{
    /// <summary>
    /// ViewModel для отображения истории донатов с кнопкой возврата.
    /// </summary>
    internal class DonateHistoryViewModel : NotifablePropertyChanged
    {
        private ObservableCollection<Donation> _donations;

        public ObservableCollection<Donation> Donations
        {
            get => _donations;
            set { _donations = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Команда возврата на предыдущую страницу.
        /// </summary>
        public ICommand GoBackCommand { get; }

        public DonateHistoryViewModel(int? channelId = null)
        {
            var context = ApplicationContext.GetInstance();
            IQueryable<Donation> query = context.Donates
                .Include(d => d.User)
                .Include(d => d.LiveStream)
                .ThenInclude(s => s.Channel);

            if (channelId.HasValue)
                query = query.Where(d => d.LiveStream.StreamChannelId == channelId.Value);

            Donations = new ObservableCollection<Donation>(query.ToList());

            GoBackCommand = new RelayCommand(_ => MainWindow.GetInstance().GoBack());
        }
    }
}