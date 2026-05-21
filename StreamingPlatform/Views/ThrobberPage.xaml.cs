using System.Windows.Controls;

namespace StreamingPlatform.Views;

/// <summary>
/// Логика взаимодействия для ThrobberPage.xaml
/// </summary>
public partial class ThrobberPage : Page
{
    /// <summary>
    /// Main constructor
    /// </summary>
    /// <param name="title"></param>
    public ThrobberPage(string title = "")
    {
        InitializeComponent();
        DataContext = new ThrobberViewModel(title);
    }
}