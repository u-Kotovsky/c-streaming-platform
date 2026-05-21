using System.Windows.Controls;
using StreamingPlatform.Models;

namespace StreamingPlatform.Views;

/// <summary>
/// Логика взаимодействия для ChannelReport.xaml
/// </summary>
public partial class ChannelReportPage : Page
{
    public ChannelReportPage()
    {
        InitializeComponent();
        DataContext = new ChannelReportViewModel();
    }
}