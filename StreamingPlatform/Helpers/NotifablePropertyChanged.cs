using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StreamingPlatform.Helpers
{
    /// <summary>
    /// Helper-class meant to be used as a base for each ViewModel class.
    /// </summary>
    public abstract class NotifablePropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}