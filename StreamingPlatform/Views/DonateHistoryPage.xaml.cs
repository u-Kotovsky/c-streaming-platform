using System.Windows.Controls;
using StreamingPlatform.Models;

namespace StreamingPlatform.Views;

/// <summary>
/// Логика взаимодействия для DonateHistoryPage.xaml
/// </summary>
public partial class DonateHistoryPage : Page
{
    public DonateHistoryPage()
    {
        InitializeComponent();
        DataContext = new DonateHistoryViewModel();
    }
}
