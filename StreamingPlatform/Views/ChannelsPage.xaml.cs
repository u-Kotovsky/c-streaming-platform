using System.Windows.Controls;
using StreamingPlatform.Models;

namespace StreamingPlatform.Views;

/// <summary>
/// Логика взаимодействия для ChannelsPage.xaml
/// </summary>
public partial class ChannelsPage : Page
{
    public ChannelsPage()
    {
        InitializeComponent();
        DataContext = new ChannelsViewModel();
    }
}