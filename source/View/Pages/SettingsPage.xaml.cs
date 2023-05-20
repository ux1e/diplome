using Source.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace source.View.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void bGoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void bChangeTheme_Click(object sender, RoutedEventArgs e)
        {
            ThemeChanger.ChangeTheme();
        }

        private void bTestWindow_Click(object sender, RoutedEventArgs e)
        {
            Source.View.Windows.TestMainWindow testMainWindow = new Source.View.Windows.TestMainWindow();   
            testMainWindow.Show();  
        }
    }
}
