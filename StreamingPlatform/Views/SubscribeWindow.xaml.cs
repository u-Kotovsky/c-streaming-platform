using System.Windows;
using StreamingPlatform.Models;

namespace StreamingPlatform.Views
{
    /// <summary>
    /// Окно оформления подписки на канал.
    /// </summary>
    public partial class SubscribeWindow : Window
    {
        /// <summary>
        /// Конструктор окна подписки.
        /// </summary>
        /// <param name="channelId">Идентификатор канала.</param>
        public SubscribeWindow(int channelId)
        {
            InitializeComponent();
            DataContext = new SubscribeViewModel(channelId);
        }
    }
}