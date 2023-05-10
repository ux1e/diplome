using Source.Core;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for SZVMPage.xaml
    /// </summary>
    public partial class SZVMPage : Page
    {
        public SZVMPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            lINN.Content = DataClass.GetInsurerINN();
            lName.Content = DataClass.GetInsurerShortName();
        }

        private void bMakeReport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void bGoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
