using System.Windows;
using System.Windows.Controls;
using StreamingPlatform.Models;

namespace StreamingPlatform
{
    public partial class MainWindow : Window
    {
        private static MainWindow? _instance;
        private readonly Stack<Page> _navigationHistory = new();

        public MainWindow()
        {
            _instance = this;
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        public static MainWindow GetInstance()
        {
            if (_instance == null)
                throw new Exception("MainWindow's instance is null.");
            return _instance;
        }

        /// <summary>
        /// Переход на страницу с сохранением истории.
        /// </summary>
        public void Navigate(Page page)
        {
            if (MainFrame.Content is Page current)
                _navigationHistory.Push(current);
            MainFrame.Navigate(page);
        }

        /// <summary>
        /// Возврат на предыдущую страницу.
        /// </summary>
        public void GoBack()
        {
            if (_navigationHistory.Count > 0)
                MainFrame.Navigate(_navigationHistory.Pop());
        }

        /// <summary>
        /// Можно ли вернуться назад.
        /// </summary>
        public bool CanGoBack => _navigationHistory.Count > 0;
    }
}