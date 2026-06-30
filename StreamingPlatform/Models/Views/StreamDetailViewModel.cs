using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using StreamingPlatform.Helpers;
using StreamingPlatform.Views;
using StreamingPlatformCore;
using StreamingPlatformCore.Entities;

namespace StreamingPlatform.Models
{
    /// <summary>
    /// ViewModel для страницы деталей трансляции.
    /// Отображает информацию о стриме, список донатов и сообщений чата.
    /// </summary>
    public class StreamDetailViewModel : NotifablePropertyChanged
    {
        private readonly int _streamId;
        private readonly ApplicationContext _context;

        private LiveStream? _stream;
        private ObservableCollection<Donation> _donations;
        private ObservableCollection<ChatMessage> _chatMessages;

        /// <summary>
        /// Текущая трансляция со всеми связанными данными.
        /// </summary>
        public LiveStream? Stream
        {
            get => _stream;
            set { _stream = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Список донатов, полученных во время трансляции.
        /// </summary>
        public ObservableCollection<Donation> Donations
        {
            get => _donations;
            set { _donations = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Сообщения чата трансляции.
        /// </summary>
        public ObservableCollection<ChatMessage> ChatMessages
        {
            get => _chatMessages;
            set { _chatMessages = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Команда возврата на предыдущую страницу.
        /// </summary>
        public ICommand GoBackCommand { get; }

        /// <summary>
        /// Команда открытия окна отправки доната.
        /// </summary>
        public ICommand DonateCommand { get; }

        /// <summary>
        /// Команда открытия окна редактирования трансляции.
        /// </summary>
        public ICommand EditStreamCommand { get; }

        /// <summary>
        /// Инициализирует ViewModel для указанной трансляции.
        /// </summary>
        /// <param name="streamId">Идентификатор трансляции.</param>
        public StreamDetailViewModel(int streamId)
        {
            _streamId = streamId;
            _context = ApplicationContext.GetInstance();

            // Инициализация команд
            GoBackCommand = new RelayCommand(_ => MainWindow.GetInstance().GoBack());
            DonateCommand = new RelayCommand(_ => OpenDonateWindow());
            EditStreamCommand = new RelayCommand(_ => OpenEditStreamWindow());

            // Загрузка данных
            LoadStream();
        }

        /// <summary>
        /// Загружает (или перезагружает) все данные трансляции из базы.
        /// Вызывается при первом открытии и после изменений (донат, редактирование).
        /// </summary>
        public void LoadStream()
        {
            _stream = _context.LiveStreams
                .Include(s => s.Channel)
                .Include(s => s.Donations)
                    .ThenInclude(d => d.User)
                .Include(s => s.ChatMessages)
                    .ThenInclude(c => c.User)
                .FirstOrDefault(s => s.Id == _streamId);

            if (_stream == null)
            {
                MessageBox.Show($"Трансляция с ID {_streamId} не найдена.",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                Donations = new ObservableCollection<Donation>();
                ChatMessages = new ObservableCollection<ChatMessage>();
                Stream = null;
                return;
            }

            Stream = _stream;
            Donations = new ObservableCollection<Donation>(_stream.Donations ?? Enumerable.Empty<Donation>());
            ChatMessages = new ObservableCollection<ChatMessage>(_stream.ChatMessages ?? Enumerable.Empty<ChatMessage>());
        }

        /// <summary>
        /// Открывает окно отправки доната и обновляет список после закрытия.
        /// </summary>
        private void OpenDonateWindow()
        {
            var window = new DonateWindow(_streamId);
            window.Owner = Application.Current.MainWindow;

            // Показываем окно и ждём результата
            bool? result = window.ShowDialog();

            // Если донат был отправлен (DialogResult = true), обновляем список
            if (result == true)
            {
                LoadStream();
            }
        }

        /// <summary>
        /// Открывает окно редактирования трансляции и обновляет данные после сохранения.
        /// </summary>
        private void OpenEditStreamWindow()
        {
            var window = new EditStreamWindow(_streamId);
            window.Owner = Application.Current.MainWindow;

            // Показываем окно и ждём результата
            bool? result = window.ShowDialog();

            // Если изменения сохранены (DialogResult = true), обновляем данные
            if (result == true)
            {
                LoadStream();
            }
        }
    }
}