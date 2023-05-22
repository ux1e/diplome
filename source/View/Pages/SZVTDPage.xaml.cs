using Source.Core;
using Source.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Xml.Linq;

namespace Source.View.Pages
{
    /// <summary>
    /// Interaction logic for SZVTDPage.xaml
    /// </summary>
    public partial class SZVTDPage : Page
    {
        public ObservableCollection<FormsSZV_TD> FormSZV_TD;

        public SZVTDPage()
        {
            InitializeComponent();

            //FormSZV_TD = new ObservableCollection<FormsSZV_TD>(DataClass.DataBase.FormsSZV_TD);
            //SZVTDGrid.ItemsSource = FormSZV_TD;
        }

        public void UpdateGrid(FormsSZV_TD f)
        {
            if ((f == null) && (SZVTDGrid.ItemsSource != null))
                f = (FormsSZV_TD)SZVTDGrid.SelectedItem;

            FormSZV_TD = new ObservableCollection<FormsSZV_TD>(DataClass.DataBase.FormsSZV_TD);
            SZVTDGrid.ItemsSource = FormSZV_TD;
            SZVTDGrid.SelectedItem = f;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            lINN.Content = Utils.GetFormatedInsurerINN();
            lName.Content = Utils.GetInsurerShortName();
        }

        private void bGoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void bMakeReport_Click(object sender, RoutedEventArgs e)
        {
            Utils.MakeSZVTDXml(DataClass.CurrentFormsSZV_TD.Id);
        }

        private void bChangeInsurer(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new InsurersPage());
        }

        private void bShowUsers_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new WorkersPage());
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
                UpdateGrid(DataClass.CurrentFormsSZV_TD);
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
                    UpdateGrid(DataClass.CurrentFormsSZV_TD);
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
