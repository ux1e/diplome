using Source.Core;
using Source.Model;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Source.View.Pages
{
    /// <summary>
    /// Interaction logic for AddNewUser.xaml
    /// </summary>
    public partial class AddNewUser : Page
    {
        public AddNewUser()
        {
            InitializeComponent();
        }

        private void bCreate_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrEmpty(tbLogin.Text))
                errors.AppendLine("Укажите логин");

            if (errors.Length > 0)
            {
                MessageBox.Show($"Произошла ошибка: {errors}");
                return;
            }

            string randomPassword = Utils.GetRandomPassword(7);

            User NewItem = new User();
            NewItem.Name = tbLogin.Text;
            NewItem.Mail = tbMail.Text;
            NewItem.Password = (bool)cbGeneratePassword.IsChecked ? randomPassword : pbPassword.Password;

            DataClass.DataBase.Users.Add(NewItem);
            DataClass.SelectedUser = NewItem;

            try
            {
                DataClass.DataBase.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка!!!!\n{ex}\nОбратитесь к администратору!!!!");
            }
            finally
            {
                MessageBox.Show($"Логин: {DataClass.SelectedUser.Name}\nПароль: {DataClass.SelectedUser.Password}",
                    "Пользователь создан");
            }
        }

        private void cbGeneratePassword_Checked(object sender, RoutedEventArgs e)
        {
            pbPassword.Visibility = !(bool)cbGeneratePassword.IsChecked ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
