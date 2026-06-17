using StreamingPlatform.Helpers;

namespace StreamingPlatform.Views;

/// <summary>
/// ViewModel logic for Throbber page.
/// </summary>
public class ThrobberViewModel : NotifablePropertyChanged
{
    #region Properties
    public string? _message;
    public string Message
    {
        get { return _message; }
        set
        {
            _message = value;
            OnPropertyChanged(nameof(Message));
        }
    }
    #endregion

    /// <summary>
    /// Main constructor
    /// </summary>
    /// <param name="message"></param>
    public ThrobberViewModel(string? message = null)
    {
        // todo: put random text

        if (message != null)
        {
            Message = message;
        }
        else
        {
            Message = "Загрузка";
        }
    }
}