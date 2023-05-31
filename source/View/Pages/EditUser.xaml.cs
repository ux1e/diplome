using Source.Core;
using Source.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Source.View.Pages
{
    /// <summary>
    /// Interaction logic for EditUser.xaml
    /// </summary>
    public partial class EditUser : Page
    {
        public EditUser()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            tbLogin.Text = DataClass.SelectedUser.Name;
            pbPassword.Password = DataClass.SelectedUser.Password;
            tbMail.Text = DataClass.SelectedUser.Mail;
            cbDarkTheme.IsChecked = DataClass.SelectedUser.IsUsingDarkMode;
        }
        private void bEdit_Click(object sender, RoutedEventArgs e)
        {
            var EditItem = new User();
            EditItem = DataClass.DataBase.Users.FirstOrDefault(p => p.Id == DataClass.SelectedUser.Id);
            EditItem.Name = tbLogin.Text;
            EditItem.Mail = tbMail.Text;
            EditItem.Password = pbPassword.Password;
            EditItem.IsUsingDarkMode = cbDarkTheme.IsChecked;

            try
            {
                DataClass.DataBase.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка!!!!\n{ex}\nОбратитесь к администратору!!!!");
            }
        }
    }
}
