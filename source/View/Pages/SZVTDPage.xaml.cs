using Source.Core;
using Source.Model;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Source.View.Pages
{
    /// <summary>
    /// Interaction logic for SZVTDPage.xaml
    /// </summary>
    public partial class SZVTDPage : Page
    {
        public ObservableCollection<FormsSZV_TD> FormSZV_TD;
        public ObservableCollection<FormsSZV_TD_Stuff> FormSZV_TD_Stuff;

        public SZVTDPage()
        {
            InitializeComponent();
        }

        private void UpdateSZVTDGrid(FormsSZV_TD f)
        {
            if ((f == null) && (SZVTDGrid.ItemsSource != null))
                f = (FormsSZV_TD)SZVTDGrid.SelectedItem;

            FormSZV_TD = new ObservableCollection<FormsSZV_TD>(DataClass.DataBase.FormsSZV_TD);
            SZVTDGrid.ItemsSource = FormSZV_TD;
            SZVTDGrid.SelectedItem = f;
        }

        private void UpdateWorkersGrid(FormsSZV_TD_Stuff f)
        {
            if ((f == null) && (WorkerGrid.ItemsSource != null))
                f = (FormsSZV_TD_Stuff)WorkerGrid.SelectedItem;

            FormSZV_TD_Stuff = new ObservableCollection<FormsSZV_TD_Stuff>(DataClass.DataBase.FormsSZV_TD_Stuff);
            WorkerGrid.ItemsSource = FormSZV_TD;
            WorkerGrid.SelectedItem = f;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            lINN.Content = Utils.GetInsurerRegNumber(true);
            lName.Content = Utils.GetInsurerShortName();

            if (DataClass.SelectedInsurer != null)
            {
                SZVTDGrid.ItemsSource = DataClass.DataBase.FormsSZV_TD.Where(list => list.InsurerID == DataClass.SelectedInsurer.Id).ToList();
            }
        }

        private void bMakeReport_Click(object sender, RoutedEventArgs e)
        {
            DataClass.CurrentFormsSZV_TD = (FormsSZV_TD)SZVTDGrid.SelectedItem;

            if (DataClass.CurrentFormsSZV_TD != null)
                Utils.MakeSZVTDXml(DataClass.CurrentFormsSZV_TD.Id);
            else MessageBox.Show("Форма null");
        }

        private void bChangeInsurer(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new InsurersPage());
        }

        private void bShowUsers_Click(object sender, RoutedEventArgs e)
        {
            if (SZVTDGrid.SelectedItem == null)
            {
                MessageBox.Show("Нужно выбрать форму", "Предупрждение");
                return;
            }

            DataClass.CurrentFormsSZV_TD = (FormsSZV_TD)SZVTDGrid.SelectedItem;

            NavigationService.Navigate(new WorkersPage(2));
        }

        private void bAddSZVTDForms_Click(object sender, RoutedEventArgs e)
        {
            if (DataClass.SelectedInsurer == null)
            {
                MessageBox.Show("Сначала нужно выбрать страхователя!", "Предупреждение", MessageBoxButton.OK);
                return;
            }

            FormsSZV_TD NewForm = new FormsSZV_TD();
            NewForm.InsurerID = DataClass.SelectedInsurer.Id;
            NewForm.DateFilling = DateTime.Now;
            NewForm.Year = DateTime.Now.Year;
            NewForm.Month = DateTime.Now.Month;
            NewForm.WorkersCount = 0;

            DataClass.DataBase.FormsSZV_TD.Add(NewForm);
            DataClass.CurrentFormsSZV_TD = NewForm;

            try
            {
                DataClass.DataBase.SaveChanges();
                UpdateSZVTDGrid(DataClass.CurrentFormsSZV_TD);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка!!!!\n{ex}\nОбратитесь к администратору!!!!");
            }
        }

        private void bDeleteSZVTDForms_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить форму?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                try
                {
                    FormsSZV_TD DeletingItem = (FormsSZV_TD)SZVTDGrid.SelectedItem;
                    if (SZVTDGrid.SelectedIndex < SZVTDGrid.Items.Count - 1)
                        SZVTDGrid.SelectedIndex++;
                    else
                    {
                        if (SZVTDGrid.SelectedIndex > 0)
                            SZVTDGrid.SelectedIndex--;
                    }
                    DataClass.CurrentFormsSZV_TD = (FormsSZV_TD)SZVTDGrid.SelectedItem;
                    DataClass.DataBase.FormsSZV_TD.Remove(DeletingItem);
                    DataClass.DataBase.SaveChanges();
                    UpdateSZVTDGrid(DataClass.CurrentFormsSZV_TD);
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
            if (MessageBox.Show("Удалить человека?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                try
                {
                    FormsSZV_TD_Stuff DeletingItem = (FormsSZV_TD_Stuff)WorkerGrid.SelectedItem;
                    if (WorkerGrid.SelectedIndex < WorkerGrid.Items.Count - 1)
                        WorkerGrid.SelectedIndex++;
                    else
                    {
                        if (WorkerGrid.SelectedIndex > 0)
                            WorkerGrid.SelectedIndex--;
                    }
                    DataClass.CurrentFormsSZV_TD_Stuff = (FormsSZV_TD_Stuff)WorkerGrid.SelectedItem;
                    DataClass.DataBase.FormsSZV_TD_Stuff.Remove(DeletingItem);
                    DataClass.DataBase.SaveChanges();
                    UpdateWorkersGrid(DataClass.CurrentFormsSZV_TD_Stuff);
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

        private void bOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(Utils.GetExePath()))
            {
                Process.Start("explorer.exe", Utils.GetExePath());
            }
        }
    }
}
