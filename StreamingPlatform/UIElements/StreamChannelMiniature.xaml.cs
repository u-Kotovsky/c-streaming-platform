using System.ComponentModel;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using StreamingPlatform.Helpers;
using StreamingPlatform.Models;

namespace StreamingPlatform.UIElements
{
    /// <summary>
    /// Interaction logic for StreamChannelMiniature.xaml
    /// </summary>
    public partial class StreamChannelMiniature : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        #region Properties
        // dependencies
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(StreamChannelMiniature));

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register(nameof(Description), typeof(string), typeof(StreamChannelMiniature));

        public static readonly DependencyProperty PosterProperty =
            DependencyProperty.Register(nameof(Poster), typeof(string), typeof(StreamChannelMiniature));

        public string _title = "DefaultName";
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public string _description = "DefaultDescription";
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public string _poster = "DefaultImage";
        public string Poster
        {
            get { return _poster; }
            set
            {
                _poster = value;
                OnPropertyChanged(nameof(Poster));
            }
        }

        public Brush _color = new SolidColorBrush(System.Windows.Media.Color.FromRgb((byte)Random.Shared.Next(0, 255), (byte)Random.Shared.Next(0, 255), (byte)Random.Shared.Next(0, 255)));
        public Brush Color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged(nameof(Color));
            }
        }
        #endregion

        private RelayCommand interact;
        public RelayCommand Interact
        {
            get
            {
                return interact ??= new RelayCommand((obj) =>
                {
                    ((InteractableModel)DataContext).Interact();
                    MessageBox.Show($"core interact!");
                    //OnInteract?.Invoke(this);
                });
            }
            set
            {
                // ignore
            }
        }

        //public event Action<StreamChannelMiniature> OnInteract = delegate {};

        /// <summary>
        /// Constructor
        /// </summary>
        public StreamChannelMiniature()
        {
            InitializeComponent();
        }
    }
}
