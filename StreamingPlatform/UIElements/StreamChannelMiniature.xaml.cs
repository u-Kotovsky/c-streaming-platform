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
    /// Миниатюра канала для отображения в списке. Поддерживает клик для перехода на карточку канала.
    /// </summary>
    public partial class StreamChannelMiniature : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #region Properties
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(StreamChannelMiniature));
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register(nameof(Description), typeof(string), typeof(StreamChannelMiniature));
        public static readonly DependencyProperty PosterProperty =
            DependencyProperty.Register(nameof(Poster), typeof(string), typeof(StreamChannelMiniature));

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
        /// Команда, вызываемая при клике на миниатюру. Открывает детальную страницу канала.
        /// </summary>
        public RelayCommand Interact
        {
            get
            {
                return Interact = new RelayCommand(obj =>
                {
                    if (DataContext is StreamChannelModel model)
                    {
                        // убедимся, что Id заполнен
                        if (model.Id == 0)
                            MessageBox.Show("Ошибка: у модели канала отсутствует Id!");
                        else
                            MainWindow.GetInstance().Navigate(new ChannelCardPage(model.Id));
                    }
                });
            }
            set { /* игнорируем, команда неизменяема */ }
        }

        public StreamChannelMiniature()
        {
            InitializeComponent();
        }
    }
}