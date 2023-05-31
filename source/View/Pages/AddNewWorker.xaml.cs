using Source.Core;
using Source.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
    /// Interaction logic for AddNewWorker.xaml
    /// </summary>
    public partial class AddNewWorker : Page
    {
        public AddNewWorker()
        {
            InitializeComponent();
        }

        private void bCreate_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrEmpty(tbFirstName.Text))
                errors.AppendLine("Укажите Имя");

            if (string.IsNullOrEmpty(tbLastName.Text))
                errors.AppendLine("Укажите Фамилию");

            if (string.IsNullOrEmpty(tbMiddleName.Text))
                errors.AppendLine("Укажите Отчество");

            if (string.IsNullOrEmpty(tbInsuranceNumber.Text))
                errors.AppendLine("Укажите СНИЛС");

            if (string.IsNullOrEmpty(tbINN.Text))
                errors.AppendLine("Укажите ИНН");

            if (((Sex)cbSex.SelectedItem == null) || (cbSex.Text == " ..."))
                errors.AppendLine("Укажите пол");

            if (((Profession)cbProfession.SelectedItem == null) || (cbProfession.Text == " ..."))
                errors.AppendLine("Укажите профессию");

            if (errors.Length > 0)
            {
                MessageBox.Show($"Произошла ошибка: {errors}");
                return;
            }

            Worker NewItem = new Worker();
            NewItem.FirstName = tbFirstName.Text;
            NewItem.LastName = tbLastName.Text;
            NewItem.MiddleName = tbMiddleName.Text;
            NewItem.InsuranceNumber = long.Parse(tbInsuranceNumber.Text);
            NewItem.INN = long.Parse(tbINN.Text);
            NewItem.Sex = cbSex.SelectedIndex;
            NewItem.Profession = cbProfession.SelectedIndex;
            NewItem.IsFired = bIsFired.IsChecked;

            DataClass.DataBase.Workers.Add(NewItem);
            DataClass.SelectedWorker = NewItem;

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
                MessageBox.Show("Работник создан");
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            cbSex.ItemsSource = DataClass.DataBase.Sexes.ToList();
            cbProfession.ItemsSource = DataClass.DataBase.Professions.ToList();
        }
    }
}
