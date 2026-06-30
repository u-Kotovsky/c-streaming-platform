using System.Windows;
using StreamingPlatform.Models;

namespace StreamingPlatform.Views
{
    public partial class EditChannelWindow : Window
    {
        public EditChannelWindow(int channelId)
        {
            InitializeComponent();
            DataContext = new EditChannelViewModel(channelId);
        }
    }
}