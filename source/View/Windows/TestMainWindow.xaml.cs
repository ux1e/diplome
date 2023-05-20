using Source.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Source.View.Windows
{
    /// <summary>
    /// Interaction logic for TestMainWindow.xaml
    /// </summary>
    public partial class TestMainWindow : Window
    {
        public TestMainWindow()
        {
            InitializeComponent();
            ThemeChanger.SetTheme(DataClass.UserInfo.IsUsingDarkMode ?? false);
            tbUserWelcome.Text += DataClass.UserInfo.Name;
        }

        private void Window_Drag(object sender, MouseButtonEventArgs e) => DragMove();

        private void ToggleTheme(object sender, RoutedEventArgs e) => ThemeChanger.ChangeTheme();

        private void CloseApp(object sender, RoutedEventArgs e) => Close();

        private void MinimizeApp(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void TabChanged(object sender, RoutedEventArgs e)
        {
            RadioButton radioBtn = (RadioButton)sender;
            if (!radioBtn.IsEnabled)
                return;

            switch (radioBtn.Name)
            {
                case "rbSZVM":
                    RootFrame.Navigate(new Pages.SZVMPage());
                    break;
                case "rbSZVTD":
                    RootFrame.Navigate(new Pages.SZVTDPage());
                    break;
                case "rbValidation":
                    RootFrame.Navigate(new Pages.ValidationPage());
                    break;
                case "rbUsers":
                    RootFrame.Navigate(new Pages.UserListPage());
                    break;
            }
        }
    }
}
