using System.Collections.ObjectModel;
using StreamingPlatform.Helpers;
using StreamingPlatformCore;

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

        StreamChannels = [.. streamChannels2];
        LiveStreams = [.. liveStreams2];
    }

    public ObservableCollection<StreamChannelModel> StreamChannels { get; set; }
    public ObservableCollection<LiveStreamModel> LiveStreams { get; set; }
}