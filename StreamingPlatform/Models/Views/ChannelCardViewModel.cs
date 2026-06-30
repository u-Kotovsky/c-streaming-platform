using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using StreamingPlatform.Helpers;
using StreamingPlatform.Views;
using StreamingPlatformCore;
using StreamingPlatformCore.Entities;
using StreamingPlatformCore.Services;

namespace StreamingPlatform.Models
{
    internal class ChannelCardViewModel : NotifablePropertyChanged
    {
        private readonly int _channelId;
        private readonly ApplicationContext _context;
        private readonly ChannelReportService _reportService;

        private StreamChannel? _channel;
        private ChannelReport? _report;

        public StreamChannel? Channel
        {
            get => _channel;
            set { _channel = value; OnPropertyChanged(); OnPropertyChanged(nameof(ChannelName)); OnPropertyChanged(nameof(Description)); OnPropertyChanged(nameof(AuthorName)); OnPropertyChanged(nameof(SubscriberCount)); }
        }

        public ChannelReport? Report
        {
            get => _report;
            set { _report = value; OnPropertyChanged(); }
        }

        public string ChannelName => Channel?.Name ?? "Без названия";
        public string Description => Channel?.Description ?? "";
        public string AuthorName => Channel?.Author?.Username ?? "Неизвестный";
        public uint SubscriberCount => Channel?.SubscriberCount ?? 0;

        public ICommand SubscribeCommand { get; }
        public ICommand GenerateReportCommand { get; }
        public ICommand ShowDonationHistoryCommand { get; }
        public ICommand EditChannelCommand { get; }
        public ICommand GoBackCommand { get; }

        public ChannelCardViewModel(int channelId)
        {
            _channelId = channelId;
            _context = ApplicationContext.GetInstance();
            _reportService = new ChannelReportService();

            SubscribeCommand = new RelayCommand(_ => OpenSubscribeWindow());
            GenerateReportCommand = new RelayCommand(_ => GenerateReport());
            ShowDonationHistoryCommand = new RelayCommand(_ => ShowDonationHistory());
            EditChannelCommand = new RelayCommand(_ => OpenEditChannel());
            GoBackCommand = new RelayCommand(_ => MainWindow.GetInstance().GoBack());

            LoadChannel();
        }

        private void LoadChannel()
        {
            Channel = _context.StreamChannels
                .Include(c => c.Author)
                .FirstOrDefault(c => c.Id == _channelId);
            if (Channel == null)
            {
                MessageBox.Show($"Канал с ID {_channelId} не найден в базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void GenerateReport()
        {
            try
            {
                Report = _reportService.GenerateChannelReport(_channelId);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                Report = null;
            }
        }

        private void OpenSubscribeWindow()
        {
            var window = new SubscribeWindow(_channelId);
            window.Owner = Application.Current.MainWindow;
            window.ShowDialog();
            LoadChannel(); // обновить подписчиков
        }

        private void ShowDonationHistory()
        {
            MainWindow.GetInstance().Navigate(new DonateHistoryPage(_channelId));
        }

        private void OpenEditChannel()
        {
            var window = new EditChannelWindow(_channelId);
            window.Owner = Application.Current.MainWindow;
            if (window.ShowDialog() == true)
                LoadChannel();
        }
    }
}