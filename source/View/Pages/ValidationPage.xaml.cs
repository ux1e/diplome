using Source.Core;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Source.View.Pages
{
    /// <summary>
    /// Interaction logic for ValidationPage.xaml
    /// </summary>
    public partial class ValidationPage : Page
    {
        private string _xsdPath;
        private string _xmlPath;

        public ValidationPage()
        {
            InitializeComponent();
        }

        private void bSelectXML_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "XML files (*.xml)|*.xml";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                _xmlPath = dialog.FileName;
                string filename = Path.GetFileNameWithoutExtension(dialog.FileName);
                tbSelectedXML.Text = filename;
            }
        }

        private void bSelectXSD_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "XSD files (*.xsd)|*.xsd";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                _xsdPath = dialog.FileName;
                string filename = Path.GetFileNameWithoutExtension(dialog.FileName);
                tbSelectedXSD.Text = filename;
            }
        }

        private void bDoValidation_Click(object sender, RoutedEventArgs e)
        {
            if (XMLValidator.ValidateXmlAgainstXsd(_xmlPath, _xsdPath))
            {
                tbResult.Text = "Файл правильный";
            }
            else
            {
                tbResult.Text = "Файл имеет некоторые проблемы :(";
            }
        }
    }
}
