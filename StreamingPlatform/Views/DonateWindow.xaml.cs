using System.Windows;
using StreamingPlatform.Models;

namespace StreamingPlatform.Views
{
    /// <summary>
    /// Окно отправки доната во время трансляции.
    /// </summary>
    public partial class DonateWindow : Window
    {
        public DonateWindow(int streamId)
        {
            InitializeComponent();
            DataContext = new DonateViewModel(streamId);
        }
    }
}