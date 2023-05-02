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
using System.Xml;

namespace Source.View.Pages
{
    /// <summary>
    /// Interaction logic for InsurersPage.xaml
    /// </summary>
    public partial class InsurersPage : Page
    {
        private int DlgMode;
        public ObservableCollection<Insurer> Insurer;

        public InsurersPage()
        {
            InitializeComponent();
            UpdateGrid(null);
            DlgLoad(false, "");
            DataContext = this;
        }

        public void UpdateGrid(Insurer u)
        {
            if ((u == null) && (Grid.ItemsSource != null))
                u = (Insurer)Grid.SelectedItem;

            Insurer = new ObservableCollection<Insurer>(DataClass.DataBase.Insurers);
            Grid.ItemsSource = Insurer;
            Grid.SelectedItem = u;
        }

        public void DlgLoad(bool b, string DlgModeContent)
        {
            FuncPanel.Width = new GridLength(b ? 200 : 0);
            Grid.IsHitTestVisible = !b;
            if (b == true)
            {
                //NameLabel.Content = DlgModeContent;
                DlgMode = -1;
            }
            //bAdd.IsEnabled = !b;
            //bCopy.IsEnabled = !b;
            //FilterComboBox.IsEnabled = !b;
            //FilterTextBox.IsEnabled = !b;
        }

        private void bSelectInsurer_Click(object sender, RoutedEventArgs e)
        {
            DataClass.SelectedInsurer = Grid.SelectedItem as Insurer;
            MessageBox.Show($"Выбран страховщик: {DataClass.SelectedInsurer.Name}");
        }

        private void bMakeNewInsurer_Click(object sender, RoutedEventArgs e)
        {
            DlgLoad(true, "Make new Isurer");
            DataContext = null;
            DlgMode = 0;
        }

        private void bGoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void AddCommit_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrEmpty(NameTextBox.Text))
                errors.AppendLine("Укажите имя");

            if (errors.Length > 0)
            {
                MessageBox.Show($"Произошла ошибка: {errors}");
                return;
            }

            if (DlgMode == 0)
            {
                var NewItem = new Insurer();
                NewItem.Name = NameTextBox.Text;                
                DataClass.DataBase.Insurers.Add(NewItem);
                DataClass.SelectedInsurer = NewItem;
            }
            else
            {
                //var EditItem = new Users();
                //EditItem = SourceCore.eRepairBase.Users.First(p => p.id == SelectedUser.id);
                //EditItem.Name = NameTextBox.Text;
                //EditItem.Phone = PhoneTextBox.Text;
                //EditItem.Mail = MailTextBox.Text;
                //EditItem.UsersStatus = (UsersStatus)TypeComboBox.SelectedItem;
            }

            try
            {
                DataClass.DataBase.SaveChanges();
                DlgLoad(false, "");
                UpdateGrid(DataClass.SelectedInsurer);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка!!!!\n{ex}\nОбратитесь к администратору!!!!");
            }
        }

        private void AddRollback_Click(object sender, RoutedEventArgs e)
        {
            DlgLoad(false, "");
            UpdateGrid(DataClass.SelectedInsurer);
        }
    }
}
