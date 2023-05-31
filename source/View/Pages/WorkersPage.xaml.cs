using Source.Core;
using Source.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Source.View.Pages
{
    /// <summary>
    /// Interaction logic for WorkersPage.xaml
    /// </summary>
    public partial class WorkersPage : Page
    {
        public ObservableCollection<Worker> Worker;
        private DataClass.FormEnum _form = 0; // 1 - SZVM; 2 - SZVTD;

        public WorkersPage(int form)
        {
            InitializeComponent();
            FuncPanel.Width = new GridLength(0);
            Worker = new ObservableCollection<Worker>(DataClass.DataBase.Workers);
            Grid.ItemsSource = Worker;
            _form = (DataClass.FormEnum)form;
        }

        private void bGoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void bSelectWorker_Click(object sender, RoutedEventArgs e)
        {
            if (Grid.SelectedItem == null)
            {
                MessageBox.Show("Нужно выбрать работника", "Предупрждение");
                return;
            }

            DataClass.SelectedWorker = (Worker)Grid.SelectedItem;

            if (_form == DataClass.FormEnum.SZV_M)
            {
                FormsSZV_M_Stuff NewForm = new FormsSZV_M_Stuff();
                NewForm.WorkerId = DataClass.SelectedWorker.Id;
                NewForm.SZV_M_Id = DataClass.CurrentFormsSZV_M.Id;

                DataClass.DataBase.FormsSZV_M_Stuff.Add(NewForm);

                var EditItem = new FormsSZV_M();
                EditItem = DataClass.CurrentFormsSZV_M;
                EditItem.WorkersCount++;
            }
            else if (_form == DataClass.FormEnum.SZV_TD)
            {
                FormsSZV_TD_Stuff NewForm = new FormsSZV_TD_Stuff();
                NewForm.WorkerId = DataClass.SelectedWorker.Id;
                NewForm.SZV_TD_Id = DataClass.CurrentFormsSZV_TD.Id;

                DataClass.DataBase.FormsSZV_TD_Stuff.Add(NewForm);

                var EditItem = new FormsSZV_TD();
                EditItem = DataClass.CurrentFormsSZV_TD;
                EditItem.WorkersCount++;
            }

            try
            {
                DataClass.DataBase.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка!!!!\n{ex}\nОбратитесь к администратору!!!!");
            }
        }

        private void bMakeNewWorker_Click(object sender, RoutedEventArgs e)
        {
            AddNewWorker addNewWorker = new AddNewWorker();
            Windows.ActionWithWorkersWindow addNewWorkerWindow = new Windows.ActionWithWorkersWindow(addNewWorker);
            addNewWorkerWindow.ShowDialog();
        }

        private void bEditWorker_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
