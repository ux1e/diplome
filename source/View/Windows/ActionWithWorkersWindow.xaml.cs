using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Source.View.Windows
{
    /// <summary>
    /// Interaction logic for ActionWithWorkersWindow.xaml
    /// </summary>
    public partial class ActionWithWorkersWindow : Window
    {
        private Page _pageForTransit;

        public ActionWithWorkersWindow(Page page)
        {
            InitializeComponent();
            _pageForTransit = page;
            tbLabel.Text = "";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) => RootFrame.Content = _pageForTransit;

        private void Window_Drag(object sender, MouseButtonEventArgs e) => DragMove();

        private void CloseApp(object sender, RoutedEventArgs e) => Close();

        private void MinimizeApp(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void ToggleTheme(object sender, RoutedEventArgs e)
        {

        }
    }
}
