using System.Windows;
using StreamingPlatform.Helpers;
using StreamingPlatformCore;
using StreamingPlatformCore.Entities;

namespace StreamingPlatform.Models;

/// <summary>
/// Main place where user selects other views.
/// </summary>
internal class MasterViewModel : NotifablePropertyChanged
{
    private readonly MainWindow _mainWindow;

    private readonly ApplicationContext _context;

    /// <summary>
    /// Main constructor
    /// </summary>
    public MasterViewModel()
    {
        _mainWindow = MainWindow.GetInstance();
        _context = ApplicationContext.GetInstance();

        PopulateLists();
    }

    private void PopulateLists()
    {
        var streamChannels = _context.StreamChannels.ToList();
        var liveStreams = _context.LiveStreams.ToList();

        var streamChannels2 = StreamChannelModel.From(streamChannels);
        var liveStreams2 = LiveStreamModel.From(liveStreams);

        StreamChannels = streamChannels2;
        LiveStreams = liveStreams2;

    }

    public List<StreamChannelModel> StreamChannels { get; set; }
    public List<LiveStreamModel> LiveStreams { get; set; }

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