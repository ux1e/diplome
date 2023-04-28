using source.Core;
using source.Model;
using System;
using System.Linq;
using System.Windows;

namespace source.View.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            try
            {
                DataClass.DataBase = new diplomeEntities();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Произошла ошибка при подключении к БД!!!\n {e} \n Обратитесь к Администратору!");
                throw;
            }
        }

        private void bExit_Click(object sender, RoutedEventArgs e) => Close();

        private bool UserAuth(string _username, string _password)
        {
            DataClass.UserInfo = DataClass.DataBase.Users.FirstOrDefault(u => u.Name == _username && u.Password == _password);
            if (DataClass.UserInfo != null) return true;
            return false;
        }

        private void bSignIn_Click(object sender, RoutedEventArgs e)
        {
            string _login = tbLogin.Text;
            string _password = pbPassword.Password;

            if (UserAuth(_login, _password))
            {
                new MainWindow().Show();
                Close();
            }
            else MessageBox.Show("Ошибка входа, првоерьте данные на корректность!");
        }
    }
}
