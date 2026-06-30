using System.Windows.Controls;
using StreamingPlatform.Models;

namespace StreamingPlatform.Views;

/// <summary>
/// Страница истории донатов с фильтрацией по каналу или общим списком.
/// </summary>
public partial class DonateHistoryPage : Page
{
    /// <summary>
    /// Создаёт страницу истории донатов для указанного канала.
    /// Если <paramref name="channelId"/> не задан, отображаются все донаты.
    /// </summary>
    /// <param name="channelId">Идентификатор канала (необязательный).</param>
    public DonateHistoryPage(int? channelId = null)
    {
        InitializeComponent();
        DataContext = new DonateHistoryViewModel(channelId);
    }
}