using System.Windows.Controls;
using StreamingPlatform.Models;

namespace StreamingPlatform.Views
{
    public partial class StreamDetailPage : Page
    {
        public StreamDetailPage(int streamId)
        {
            InitializeComponent();
            DataContext = new StreamDetailViewModel(streamId);
        }
    }
}