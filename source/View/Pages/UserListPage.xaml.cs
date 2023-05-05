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
    /// Interaction logic for UserListPage.xaml
    /// </summary>
    public partial class UserListPage : Page
    {
        private int DlgMode;
        public ObservableCollection<User> User;

        public UserListPage()
        {
            InitializeComponent();
            UpdateGrid(null);
            DlgLoad(false);
            DataContext = this;
        }

        public void UpdateGrid(User u)
        {
            if ((u == null) && (Grid.ItemsSource != null))
                u = (User)Grid.SelectedItem;

            User = new ObservableCollection<User>(DataClass.DataBase.Users);
            Grid.ItemsSource = User;
            Grid.SelectedItem = u;
        }

        public void DlgLoad(bool b)
        {
            NameTextBox.Text = "";
            MailTextBox.Text = "";

            FuncPanel.Width = new GridLength(b ? 200 : 0);
            Grid.IsHitTestVisible = !b;
            if (b == true)
            {
                DlgMode = -1;
            }
        }

        private void bGoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void bMakeNewUser_Click(object sender, RoutedEventArgs e)
        {
            DlgLoad(true);
            DataContext = null;
            DlgMode = 0;
        }

        private void AddRollback_Click(object sender, RoutedEventArgs e)
        {
            DlgLoad(false);
            UpdateGrid(null);
        }

        private void AddCommit_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrEmpty(NameTextBox.Text))
                errors.AppendLine("Укажите имя");

            //if (string.IsNullOrEmpty(MailTextBox.Text))
            //    errors.AppendLine("Укажите почту");

            if (errors.Length > 0)
            {
                MessageBox.Show($"Произошла ошибка: {errors}");
                return;
            }

            string randomPassword = DataClass.GetRandomPassword(7);
            if (DlgMode == 0)
            {
                var NewItem = new User();
                NewItem.Name = NameTextBox.Text;
                NewItem.Mail = MailTextBox.Text;
                NewItem.Password = randomPassword;
                DataClass.DataBase.Users.Add(NewItem);
                DataClass.SelectedUser = NewItem;
            }
            else
            {
                var EditItem = new User();
                EditItem = DataClass.DataBase.Users.FirstOrDefault(p => p.Id == DataClass.SelectedUser.Id);
                EditItem.Name = NameTextBox.Text;
                EditItem.Mail = MailTextBox.Text;
            }

            try
            {
                DataClass.DataBase.SaveChanges();
                DlgLoad(false);
                UpdateGrid(DataClass.SelectedUser);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка!!!!\n{ex}\nОбратитесь к администратору!!!!");
            }
            finally
            {
                if (DlgMode == 0)
                {
                    MessageBox.Show($"Логин: {DataClass.SelectedUser.Name}\nПароль: {randomPassword}", "Пользователь создан");
                }
            }
        }

        private void bDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (Grid.SelectedItem == DataClass.UserInfo)
            {
                MessageBox.Show("Стрелять конечно хорошо, но не себе же в ногу", "ПРЕДУПРЕЖДЕНИЕ!!!!!");
                return;
            }
            if (Grid.SelectedItem == DataClass.DataBase.Users.FirstOrDefault(p => p.Name == "Администратор"))
            {
                MessageBox.Show("НЕЛЬЗЯ УДАЛИТЬ АДМИНИСТРАТОРА (◣_◢)", "ПРЕДУПРЕЖДЕНИЕ!!!!!");
                return;
            }
            if (Grid.SelectedItem == null)
            {
                MessageBox.Show("Для удаления необходимо выбрать пользователя", "Информация");
            }

            if (MessageBox.Show("Удалить пользователя?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                try
                {
                    User DeletingItem = (User)Grid.SelectedItem;
                    if (Grid.SelectedIndex < Grid.Items.Count - 1)
                        Grid.SelectedIndex++;
                    else
                    {
                        if (Grid.SelectedIndex > 0)
                            Grid.SelectedIndex--;
                    }
                    DataClass.SelectedUser = (User)Grid.SelectedItem;
                    DataClass.DataBase.Users.Remove(DeletingItem);
                    DataClass.DataBase.SaveChanges();
                    UpdateGrid(DataClass.SelectedUser);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Невозможно выполнить удаление\nОшибка: {ex}.",
                        "Предупреждение",
                        MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None);
                }
            }
        }

        private void bEditUser_Click(object sender, RoutedEventArgs e)
        {
            if (Grid.SelectedItem == DataClass.DataBase.Users.FirstOrDefault(p => p.Name == "Администратор"))
            {
                MessageBox.Show("НЕЛЬЗЯ ИЗМЕНИТЬ АДМИНИСТРАТОРА (◣_◢)", "ПРЕДУПРЕЖДЕНИЕ!!!!!");
                return;
            }
            if (Grid.SelectedItem == null)
            {
                MessageBox.Show("Для удаления необходимо выбрать пользователя", "Информация");
                return;
            }

            DlgLoad(true);
            DataClass.SelectedUser = (User)Grid.SelectedItem;
            NameTextBox.Text = DataClass.SelectedUser.Name;
            MailTextBox.Text = DataClass.SelectedUser.Mail;
        }
    }
}
