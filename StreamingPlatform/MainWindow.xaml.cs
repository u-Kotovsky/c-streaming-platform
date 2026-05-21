using System.Windows;
using StreamingPlatform.Models;

namespace StreamingPlatform
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Main constructor, entry-point for window
        /// </summary>
        public MainWindow()
        {
            _instance = this;

            InitializeComponent();

            DataContext = new MainWindowViewModel();
        }

        #region Singleton
        private static MainWindow? _instance;

        /// <summary>
        /// Get current instance
        /// </summary>
        /// <returns></returns>
        public static MainWindow GetInstance()
        {
            if (_instance == null)
            {
                throw new Exception("MainWindow's instance is null. That should never happen. What did you do?");
            }

            return _instance;
        }
        #endregion

        public void Navigate(object content)
        {
            MainFrame.Navigate(content);
        }
    }
}