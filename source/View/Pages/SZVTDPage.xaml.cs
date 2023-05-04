using Source.Core;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Source.View.Pages
{
    /// <summary>
    /// Interaction logic for SZVTDPage.xaml
    /// </summary>
    public partial class SZVTDPage : Page
    {
        public SZVTDPage()
        {
            InitializeComponent();

            //List<Insurer> insurerList = new List<Insurer>
            //{
            //    new Insurer {Date= new DateTime(2000, 01, 24), Year=2000, Month=1, WorkerCount=3 },
            //};
            //InsurerGrid.ItemsSource = insurerList;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataClass.SelectedInsurer == null)
            {
                lINN.Content = "[___-___-______]";
                lName.Content = " ";
            }
            else
            {
                lINN.Content = $"[{DataClass.SelectedInsurer.INN}]";
                lName.Content = DataClass.SelectedInsurer.NameShort;
            }
        }

        private void bGoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void bMakeReport_Click(object sender, RoutedEventArgs e)
        {
            string fileName = string.Format("ПФР_СЗВ-ТД_{0}_{1}.XML", DataClass.UserInfo.Name, DataClass.GetDateTime());

            XNamespace xnamespace = "http://пф.рф/СЗВ-ТД/2020-09-26";
            XNamespace xnamespace2 = "http://пф.рф/УТ/2017-08-21";
            XNamespace xnamespace3 = "http://пф.рф/ВС/ЕФС/2022-09-22";
            XNamespace xnamespace4 = "http://пф.рф/ВС/типы/2017-10-23";
            XNamespace xnamespace5 = "http://пф.рф/АФ/2018-12-07";
            XNamespace xnamespace6 = "http://www.w3.org/2001/XMLSchema-instance";

            string value = "http://пф.рф/СЗВ-ТД/2020-09-26 ../../Схемы/ЭТК/СЗВ-ТД_2020-09-26.xsd";

            XDocument xdocument = new XDocument(new XDeclaration("1.0", "UTF-8", null), new object[]
            {
                new XElement(xnamespace + "ЭДПФР", new object[]
                {
                    new XAttribute(XNamespace.Xmlns + "УТ2", xnamespace2.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "АФ5", xnamespace5.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "xsi", xnamespace6.NamespaceName),
                    new XAttribute(xnamespace6 + "schemaLocation", value)
                })
            });

            XElement xelement = new XElement(xnamespace + "Работодатель", new object[]
            {
                new XElement(xnamespace2 + "РегНомер", 123),
                new XElement(xnamespace + "НаименованиеОрганизации", 123),
                new XElement(xnamespace2 + "ИНН",
                (DataClass.SelectedInsurer == null) ? "" : DataClass.SelectedInsurer.INN.ToString())
            });
            XElement xelement2 = new XElement(xnamespace + "Страхователь", new object[]
            {
                new XElement(xnamespace3 + "РегНомер", 123),
                new XElement(xnamespace3 + "Наименование", 123),
                new XElement(xnamespace2 + "ИНН",
                (DataClass.SelectedInsurer == null) ? "" : DataClass.SelectedInsurer.INN.ToString())
            });

            XElement xelement3 = new XElement(xnamespace + "СЗВ-ТД", xelement);
            XElement xelement4 = new XElement(xnamespace + "ЕФС-1", xelement2);

            XElement xelement25 = new XElement(xnamespace + "СлужебнаяИнформация", new object[]
            {
                new XElement(xnamespace5 + "GUID", DataClass.GetUUID()),
                new XElement(xnamespace5 + "ДатаВремя", DataClass.GetDateTime())
            });

            xdocument.Element(xnamespace + "ЭДПФР").Add(xelement3);
            xdocument.Element(xnamespace + "ЭДПФР").Add(xelement25);

            xdocument.Save(fileName);
        }

        private void bChangeInsurer(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new InsurersPage());
        }

        private void bShowUsers_Click(object sender, RoutedEventArgs e)
        {
            //Windows.WorkerInfoWindow workerInfoWindow = new Windows.WorkerInfoWindow();
            //workerInfoWindow.ShowDialog();
            NavigationService.Navigate(new WorkersPage());
        }
    }
}
