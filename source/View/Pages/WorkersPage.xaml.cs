using Source.Core;
using Source.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Source.View.Pages
{
    /// <summary>
    /// Interaction logic for WorkersPage.xaml
    /// </summary>
    public partial class WorkersPage : Page
    {
        public ObservableCollection<Worker> Worker;

        public WorkersPage()
        {
            InitializeComponent();
            FuncPanel.Width = new GridLength(0);
            Worker = new ObservableCollection<Worker>(DataClass.DataBase.Workers);
            Grid.ItemsSource = Worker;
        }

        private void bGoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void bSelectWorker_Click(object sender, RoutedEventArgs e)
        {

        }

        private void bMakeNewWorker_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
