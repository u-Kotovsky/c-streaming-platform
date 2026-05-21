using System.Windows.Controls;
using StreamingPlatform.Models;

namespace StreamingPlatform.Views;

/// <summary>
/// Логика взаимодействия для LiveStreamsPage.xaml
/// </summary>
public partial class LiveStreamsPage : Page
{
    public LiveStreamsPage()
    {
        InitializeComponent();
        DataContext = new LiveStreamsViewModel();
    }
}