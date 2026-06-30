using System.Windows;
using System.Windows.Controls;
using StreamingPlatform.Models;

namespace StreamingPlatform.Views;

/// <summary>
/// Логика взаимодействия для MasterPage.xaml
/// </summary>
public partial class MasterPage : Page
{
    public MasterPage()
    {
        InitializeComponent();
        DataContext = new MasterViewModel();
    }
    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is MasterViewModel vm)
            vm.RefreshLists();
    }
}