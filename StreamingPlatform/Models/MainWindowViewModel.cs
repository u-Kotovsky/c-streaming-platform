using StreamingPlatform.Helpers;
using StreamingPlatform.Views;
using StreamingPlatformCore;
using StreamingPlatformCore.Services;

namespace StreamingPlatform.Models;

/// <summary>
/// ViewModel logic for MainWindow
/// </summary>
internal class MainWindowViewModel : NotifablePropertyChanged
{
    private ApplicationContext? _context;
    private readonly MainWindow _mainWindow;
    private readonly UserService _userService;
    private readonly ThrobberPage _throbber;
    private readonly Thread _thread;

    /// <summary>
    /// Main constructor
    /// </summary>
    public MainWindowViewModel()
    {
        _throbber ??= new ThrobberPage();
        _userService = new UserService();

        _mainWindow = MainWindow.GetInstance();
        _mainWindow.MainFrame.Navigate(_throbber);

        // runs in background thread so we can show throbber to user
        // so they know program is actually trying to launch
        // so they wait for it to finish and not accidentally
        // run multiple instances of app
        _thread = new Thread(EnsureDatabaseConnectivity) { IsBackground = true };
        _thread.Start();
    }

    private void EnsureDatabaseConnectivity()
    {
        try
        {
            _throbber.Dispatcher.Invoke(() =>
            {
                ((ThrobberViewModel)_throbber.DataContext).Message = "Подключение..";
            });

            _context = ApplicationContext.GetInstance();
        }
        catch (Exception ex)
        {
            _throbber.Dispatcher.Invoke(() =>
            {
                ((ThrobberViewModel)_throbber.DataContext).Message = ex.Message;
            });

            throw;
        }

        _throbber.Dispatcher.Invoke(() =>
        {
            ((ThrobberViewModel)_throbber.DataContext).Message = "Успех";

            _mainWindow.MainFrame.Navigate(new MasterPage());
        });
    }
}
