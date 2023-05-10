using Source.Core;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Source.View.Pages;

namespace source.View.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void bSettings_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SettingsPage());
        }

        private void bSZVTD_Click(object sender, RoutedEventArgs e)
        {   
            NavigationService.Navigate(new SZVTDPage());
        }

        private void bUserList_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UserListPage());
        }

        private void bSZVM_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SZVMPage());
        }
    }
}
