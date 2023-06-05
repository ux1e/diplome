using Source.Core;
using Source.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Source.View.Pages
{
    /// <summary>
    /// Interaction logic for SZVMPage.xaml
    /// </summary>
    public partial class SZVMPage : Page
    {
        public ObservableCollection<FormsSZV_M> FormSZV_M;
        public ObservableCollection<FormsSZV_M_Stuff> FormSZV_M_Stuff;

        public SZVMPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            lINN.Content = Utils.GetInsurerRegNumber(true);
            lName.Content = Utils.GetInsurerShortName();

            if (DataClass.SelectedInsurer != null)
            {
                SZVMGrid.ItemsSource = DataClass.DataBase.FormsSZV_M.Where(list => list.InsurerId == DataClass.SelectedInsurer.Id).ToList();
            }
        }

        private void UpdateSZVMGrid(FormsSZV_M f)
        {
            if ((f == null) && (SZVMGrid.ItemsSource != null))
                f = (FormsSZV_M)SZVMGrid.SelectedItem;

            FormSZV_M = new ObservableCollection<FormsSZV_M>(DataClass.DataBase.FormsSZV_M);
            SZVMGrid.ItemsSource = FormSZV_M;
            SZVMGrid.SelectedItem = f;
        }

        private void UpdateWorkersGrid(FormsSZV_M_Stuff f)
        {
            if ((f == null) && (WorkerGrid.ItemsSource != null))
                f = (FormsSZV_M_Stuff)WorkerGrid.SelectedItem;

            FormSZV_M_Stuff = new ObservableCollection<FormsSZV_M_Stuff>(DataClass.DataBase.FormsSZV_M_Stuff);
            WorkerGrid.ItemsSource = FormSZV_M;
            WorkerGrid.SelectedItem = f;
        }

        private void bMakeReport_Click(object sender, RoutedEventArgs e)
        {
            DataClass.CurrentFormsSZV_M = (FormsSZV_M)SZVMGrid.SelectedItem;

            if (DataClass.CurrentFormsSZV_M != null)
                Utils.MakeSZVMXml(DataClass.CurrentFormsSZV_M.Id);
            else MessageBox.Show("Форма null");
        }

        private void bGoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void bChangeInsurer(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new InsurersPage());
        }

        private void bShowUsers_Click(object sender, RoutedEventArgs e)
        {
            if (SZVMGrid.SelectedItem == null)
            {
                MessageBox.Show("Нужно выбрать форму", "Предупрждение");
                return;
            }

            DataClass.CurrentFormsSZV_M = (FormsSZV_M)SZVMGrid.SelectedItem;

            NavigationService.Navigate(new WorkersPage(1));
        }

        private void bAddSZVM_Click(object sender, RoutedEventArgs e)
        {
            if (DataClass.SelectedInsurer == null)
            {
                MessageBox.Show("Сначала нужно выбрать страхователя!", "Предупреждение", MessageBoxButton.OK);
                return;
            }

            FormsSZV_M NewForm = new FormsSZV_M();
            NewForm.InsurerId = DataClass.SelectedInsurer.Id;
            NewForm.DateFilling = DateTime.Now;
            NewForm.Year = DateTime.Now.Year;
            NewForm.Month = DateTime.Now.Month;
            NewForm.WorkersCount = 0;

            DataClass.DataBase.FormsSZV_M.Add(NewForm);
            DataClass.CurrentFormsSZV_M = NewForm;

            try
            {
                DataClass.DataBase.SaveChanges();
                UpdateSZVMGrid(DataClass.CurrentFormsSZV_M);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка!!!!\n{ex}\nОбратитесь к администратору!!!!");
            }
        }

        private void bRemoveSZVM_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить форму?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                try
                {
                    FormsSZV_M DeletingItem = (FormsSZV_M)SZVMGrid.SelectedItem;
                    if (SZVMGrid.SelectedIndex < SZVMGrid.Items.Count - 1)
                        SZVMGrid.SelectedIndex++;
                    else
                    {
                        if (SZVMGrid.SelectedIndex > 0)
                            SZVMGrid.SelectedIndex--;
                    }
                    DataClass.CurrentFormsSZV_M = (FormsSZV_M)SZVMGrid.SelectedItem;
                    DataClass.DataBase.FormsSZV_M.Remove(DeletingItem);
                    DataClass.DataBase.SaveChanges();
                    UpdateSZVMGrid(DataClass.CurrentFormsSZV_M);
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

        private void bDeleteUsers_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить работника?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                try
                {
                    FormsSZV_M_Stuff DeletingItem = (FormsSZV_M_Stuff)WorkerGrid.SelectedItem;
                    if (WorkerGrid.SelectedIndex < WorkerGrid.Items.Count - 1)
                        WorkerGrid.SelectedIndex++;
                    else
                    {
                        if (WorkerGrid.SelectedIndex > 0)
                            WorkerGrid.SelectedIndex--;
                    }
                    DataClass.CurrentFormsSZV_M_Stuff = (FormsSZV_M_Stuff)WorkerGrid.SelectedItem;
                    DataClass.DataBase.FormsSZV_M_Stuff.Remove(DeletingItem);

                    var EditItem = new FormsSZV_M();
                    EditItem = DataClass.CurrentFormsSZV_M;
                    EditItem.WorkersCount++;

                    DataClass.DataBase.SaveChanges();
                    UpdateWorkersGrid(DataClass.CurrentFormsSZV_M_Stuff);
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
    }
}
