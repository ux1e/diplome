using System.Windows;

namespace Source.View.Windows
{
    /// <summary>
    /// Interaction logic for ActionWithUserWindow.xaml
    /// </summary>
    public partial class ActionWithUserWindow : Window
    {
        private System.Windows.Controls.Page _pageForTransit;
        public ActionWithUserWindow(System.Windows.Controls.Page page)
        {
            InitializeComponent();
            _pageForTransit = page;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RootFrame.Content = _pageForTransit;
        }

        private void Window_Drag(object sender, System.Windows.Input.MouseButtonEventArgs e) => DragMove();

        private void CloseApp(object sender, RoutedEventArgs e) => Close();

        private void MinimizeApp(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
    }
}
