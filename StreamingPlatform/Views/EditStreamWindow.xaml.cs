using System.Windows;
using StreamingPlatform.Models;

namespace StreamingPlatform.Views
{
    /// <summary>
    /// Окно редактирования трансляции (название, дата, длительность, статус, канал).
    /// </summary>
    public partial class EditStreamWindow : Window
    {
        public EditStreamWindow(int streamId)
        {
            InitializeComponent();
            DataContext = new EditStreamViewModel(streamId);
        }
    }
}