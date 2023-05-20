using System.Windows;
using System.Xml;
using System.Xml.Schema;

namespace Source.Core
{
    public static class XMLValidator
    {
        private static bool isValid = true;
        public static bool ValidateXmlAgainstXsd(string xmlPath, string xsdPath)
        {
            isValid = true;
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;

            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add(null, xsdPath);

            settings.Schemas = schemas;
            settings.ValidationEventHandler += ValidationEventHandler;

            using (XmlReader reader = XmlReader.Create(xmlPath, settings))
            {
                while (reader.Read()) { }
            }

            return isValid;
        }


        private static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            MessageBox.Show("Validation error: " + e.Message);
            isValid = false;
        }
    }
}
