using source.Core;
using System.Windows;
using System;

namespace source
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Title = $"Создание отчётов | {DataClass.UserInfo.Name}";
            MainFrame.Content = new View.Pages.MainPage();
        }
    }
}
