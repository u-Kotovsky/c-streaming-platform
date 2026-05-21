using StreamingPlatform.Helpers;

namespace StreamingPlatform.Models;

/// <summary>
/// Main place where user selects other views.
/// </summary>
internal class MasterViewModel : NotifablePropertyChanged
{
    private readonly MainWindow _mainWindow;

    /// <summary>
    /// Main constructor
    /// </summary>
    public MasterViewModel()
    {
        _mainWindow = MainWindow.GetInstance();
    }

    #region Commands
    private RelayCommand? _showCatalogCommand;
    public RelayCommand ShowCatalogCommand
    {
        get
        {
            return _showCatalogCommand ??= new RelayCommand(obj =>
            {
                //_mainWindow.Navigate(new CatalogPage());
            });
        }
    }

    private RelayCommand? _showHistoryCommand;
    public RelayCommand ShowHistoryCommand
    {
        get
        {
            return _showHistoryCommand ??= new RelayCommand(obj =>
            {
                //_mainWindow.Navigate(new HistoryPage());
            });
        }
    }

    private RelayCommand? _showSubscriptionCommand;
    public RelayCommand ShowSubscriptionCommand
    {
        get
        {
            return _showSubscriptionCommand ??= new RelayCommand(obj =>
            {
                //var window = new SubscriptionManagementWindow();
                //window.ShowDialog();
            });
        }
    }
    #endregion
}
