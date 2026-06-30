using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using StreamingPlatform.Helpers;
using StreamingPlatformCore.Services;

namespace StreamingPlatform.Models
{
    /// <summary>
    /// ViewModel для окна оформления подписки.
    /// </summary>
    public class SubscribeViewModel : NotifablePropertyChanged
    {
        private readonly int _channelId;
        private readonly StreamChannelService _service;

        /// <summary>
        /// Доступные планы подписки.
        /// </summary>
        public ObservableCollection<SubscriptionPlan> Plans { get; }

        private SubscriptionPlan _selectedPlan;
        /// <summary>
        /// Выбранный план подписки.
        /// </summary>
        public SubscriptionPlan SelectedPlan
        {
            get => _selectedPlan;
            set
            {
                _selectedPlan = value;
                OnPropertyChanged(nameof(SelectedPlan));
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(DurationDays));
            }
        }

        /// <summary>
        /// Стоимость выбранного плана.
        /// </summary>
        public decimal Price => SelectedPlan?.Price ?? 0;

        /// <summary>
        /// Длительность подписки в днях.
        /// </summary>
        public int DurationDays => SelectedPlan?.DurationDays ?? 0;

        /// <summary>
        /// Команда подтверждения подписки.
        /// </summary>
        public ICommand ConfirmCommand { get; }

        /// <summary>
        /// Инициализирует ViewModel для подписки на канал.
        /// </summary>
        /// <param name="channelId">Идентификатор канала.</param>
        public SubscribeViewModel(int channelId)
        {
            _channelId = channelId;
            _service = new StreamChannelService();

            Plans = new ObservableCollection<SubscriptionPlan>
            {
                new SubscriptionPlan("1 месяц", 5.99m, 30),
                new SubscriptionPlan("3 месяца", 14.99m, 90),
                new SubscriptionPlan("12 месяцев", 49.99m, 365)
            };
            SelectedPlan = Plans[0];

            ConfirmCommand = new RelayCommand(_ => Subscribe());
        }

        private void Subscribe()
        {
            try
            {
                var userService = new UserService();
                _service.AddSubscription(_channelId, userService.CurrentUser.Id, Price, DurationDays);
                MessageBox.Show("Подписка успешно оформлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                // Закрываем окно
                if (Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.DataContext == this) is Window window)
                    window.DialogResult = true;
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }

    /// <summary>
    /// План подписки с названием, ценой и длительностью.
    /// </summary>
    public class SubscriptionPlan
    {
        public string Name { get; }
        public decimal Price { get; }
        public int DurationDays { get; }

        public SubscriptionPlan(string name, decimal price, int durationDays)
        {
            Name = name;
            Price = price;
            DurationDays = durationDays;
        }
    }
}