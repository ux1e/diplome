using source.Core;
using System.Windows;

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
            ThemeChanger.SetTheme(DataClass.UserInfo.IsUsingDarkMode ?? false);
            MainFrame.Content = new View.Pages.MainPage();
        }
    }
}
