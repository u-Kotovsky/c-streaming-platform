using System.Windows.Controls;
using StreamingPlatform.Models;

namespace StreamingPlatform.Views;

/// <summary>
/// Страница с карточкой канала: информация, подписка, отчёт, история донатов.
/// </summary>
public partial class ChannelCardPage : Page
{
    /// <summary>
    /// Создаёт страницу для указанного канала.
    /// </summary>
    /// <param name="channelId">Идентификатор канала.</param>
    public ChannelCardPage(int channelId)
    {
        InitializeComponent();
        DataContext = new ChannelCardViewModel(channelId);
    }
}