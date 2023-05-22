using Source.Core;
using Source.Model;
using System;
using System.Text;
using System.Windows;

namespace Source.View.Windows
{
    /// <summary>
    /// Interaction logic for ActionWithUserWindow.xaml
    /// </summary>
    public partial class ActionWithUserWindow : Window
    {
        public ActionWithUserWindow()
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
            NewItem.Password = pbPassword.Password.Length == 0 ? randomPassword : pbPassword.Password;

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

        private void bCancel_Click(object sender, RoutedEventArgs e) => Close();

        private void Window_Drag(object sender, System.Windows.Input.MouseButtonEventArgs e) => DragMove();
    }
}
