using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using StreamingPlatform.Helpers;
using StreamingPlatform.Models;
using StreamingPlatform.Views;

namespace StreamingPlatform.UIElements
{
    /// <summary>
    /// Миниатюра трансляции для отображения в списке. Поддерживает клик для перехода на детали стрима.
    /// </summary>
    public partial class LiveStreamMiniature : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #region Properties
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(LiveStreamMiniature));
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register(nameof(Description), typeof(string), typeof(LiveStreamMiniature));
        public static readonly DependencyProperty PosterProperty =
            DependencyProperty.Register(nameof(Poster), typeof(string), typeof(LiveStreamMiniature));

        public string _title = "DefaultName";
        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        public string _description = "DefaultDescription";
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        public string _poster = "DefaultImage";
        public string Poster
        {
            get => _poster;
            set { _poster = value; OnPropertyChanged(); }
        }

        public Brush _color = new SolidColorBrush(System.Windows.Media.Color.FromRgb((byte)Random.Shared.Next(0, 255), (byte)Random.Shared.Next(0, 255), (byte)Random.Shared.Next(0, 255)));
        public Brush Color
        {
            get => _color;
            set { _color = value; OnPropertyChanged(); }
        }
        #endregion

        private RelayCommand? interact;
        /// <summary>
        /// Команда, вызываемая при клике на миниатюру. Открывает страницу с деталями стрима.
        /// </summary>
        public RelayCommand Interact
        {
            get
            {
                return Interact = new RelayCommand(obj =>
                {
                    if (DataContext is LiveStreamModel model)
                        MainWindow.GetInstance().Navigate(new StreamDetailPage(model.Id));
                });
            }
            set { /* игнорируем */ }
        }

        public LiveStreamMiniature()
        {
            InitializeComponent();
        }
    }
}