using Source.Core;
using Source.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

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
            UpdateListView(null);
            DlgLoad(false);
            DataContext = this;
        }

        public void UpdateListView(User u)
        {
            if ((u == null) && (lvUsers.ItemsSource != null))
                u = (User)lvUsers.SelectedItem;

            User = new ObservableCollection<User>(DataClass.DataBase.Users);
            lvUsers.ItemsSource = User;
            lvUsers.SelectedItem = u;
        }

        public void DlgLoad(bool b)
        {
            lvUsers.IsHitTestVisible = !b;
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
            Windows.ActionWithUserWindow addNewUserWindow = new Windows.ActionWithUserWindow();
            addNewUserWindow.ShowDialog();
            //DlgLoad(true);
            //DataContext = null;
            //DlgMode = 0;
        }

        private void AddRollback_Click(object sender, RoutedEventArgs e)
        {
            DlgLoad(false);
            UpdateListView(null);
        }

        private void AddCommit_Click(object sender, RoutedEventArgs e)
        {
            /*
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
            */
        }

        private void bDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (lvUsers.SelectedItem == DataClass.UserInfo)
            {
                MessageBox.Show("Стрелять конечно хорошо, но не себе же в ногу", "ПРЕДУПРЕЖДЕНИЕ!!!!!");
                return;
            }
            if (lvUsers.SelectedItem == DataClass.DataBase.Users.FirstOrDefault(p => p.Name == "Администратор"))
            {
                MessageBox.Show("НЕЛЬЗЯ УДАЛИТЬ АДМИНИСТРАТОРА (◣_◢)", "ПРЕДУПРЕЖДЕНИЕ!!!!!");
                return;
            }
            if (lvUsers.SelectedItem == null)
            {
                MessageBox.Show("Для удаления необходимо выбрать пользователя", "Информация");
                return;
            }

            if (MessageBox.Show("Удалить пользователя?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                try
                {
                    User DeletingItem = (User)lvUsers.SelectedItem;
                    if (lvUsers.SelectedIndex < lvUsers.Items.Count - 1)
                        lvUsers.SelectedIndex++;
                    else
                    {
                        if (lvUsers.SelectedIndex > 0)
                            lvUsers.SelectedIndex--;
                    }
                    DataClass.SelectedUser = (User)lvUsers.SelectedItem;
                    DataClass.DataBase.Users.Remove(DeletingItem);
                    DataClass.DataBase.SaveChanges();
                    UpdateListView(DataClass.SelectedUser);
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
            /*
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
            */
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                lvUsers.ItemsSource = DataClass.DataBase.Users.Where(item => item.Name == tbSearch.Text || 
                                                                             item.Name.Contains(tbSearch.Text) ||
                                                                             item.Mail == tbSearch.Text ||
                                                                             item.Mail.Contains(tbSearch.Text)
                                                                     ).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка!!!!\n{ex}\nОбратитесь к Администратору!", "Ошибка");
                throw;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            lvUsers.ItemsSource = DataClass.DataBase.Users.ToList();
        }
    }
}
