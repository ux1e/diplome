using Source.Model;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Xml.Linq;

namespace Source.Core
{
    static internal class Utils
    {
        static public void MakeSZVTDXml(int szvtdID)
        {
            try
            {
                DataClass.CurrentFormsSZV_TD = DataClass.DataBase.FormsSZV_TD.First((FormsSZV_TD x) => x.Id == szvtdID);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: \n{ex}\nОбратитесь к администратору!", "Произошла ошибка");
                return;
            }

            if (DataClass.CurrentFormsSZV_TD == null)
            {
                MessageBox.Show("FormsSZV_TD == null", "Ошибка", (MessageBoxButton)MessageBoxImage.Error);
                return;
            }

            string fileName = string.Format("ПФР_СЗВ-ТД_{0}_{1}.XML", DataClass.UserInfo.Name, GetDateTime());

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

            string orgName = "";
            if (!string.IsNullOrEmpty(DataClass.SelectedInsurer.NameShort))
            {
                orgName = DataClass.SelectedInsurer.NameShort;
            }
            else
            {
                if (!string.IsNullOrEmpty(DataClass.SelectedInsurer.Name))
                {
                    orgName = DataClass.SelectedInsurer.Name;
                }
            }

            XElement xelement = new XElement(xnamespace + "Работодатель", new object[]
            {
                new XElement(xnamespace2 + "РегНомер", GetInsurerRegNumber()),
                new XElement(xnamespace + "НаименованиеОрганизации", orgName),
                new XElement(xnamespace2 + "ИНН", GetInsurerINN())
            });
            XElement xelement2 = new XElement(xnamespace + "Страхователь", new object[]
            {
                new XElement(xnamespace3 + "РегНомер", GetInsurerRegNumber()),
                new XElement(xnamespace3 + "НаименованиеОрганизации", orgName),
                new XElement(xnamespace2 + "ИНН", GetInsurerINN())
            });

            XElement xelement3 = new XElement(xnamespace + "СЗВ-ТД", xelement);

            XElement content = new XElement(xnamespace + "ОтчетныйПериод", new object[]
            {
                new XElement(xnamespace + "Месяц", DataClass.CurrentFormsSZV_TD.Month),
                new XElement(xnamespace + "КалендарныйГод", DataClass.CurrentFormsSZV_TD.Year)
            });
            xelement3.Add(content);

            XElement xelement4 = new XElement(xnamespace + "ЕФС-1", xelement2);

            XElement xelement25 = new XElement(xnamespace + "СлужебнаяИнформация", new object[]
            {
                new XElement(xnamespace5 + "GUID", GetUUID()),
                new XElement(xnamespace5 + "ДатаВремя", GetDateTime())
            });

            xdocument.Element(xnamespace + "ЭДПФР").Add(xelement3);
            xdocument.Element(xnamespace + "ЭДПФР").Add(xelement25);

            xdocument.Save(fileName);
        }
        public static string GetUUID()
        {
            Guid myuuid = Guid.NewGuid();
            return myuuid.ToString();
        }

        public static string GetDateTime()
        {
            DateTime currentDateTime = DateTime.Now;
            return currentDateTime.ToString("MM-dd-yyyy HH.mm.ss");
        }

        public static string ParseRegNum(long n)
        {
            string s = n.ToString();
            if (!string.IsNullOrEmpty(s) && s.Length == 12)
            {
                return string.Concat(new string[]
                {
                    s.Substring(0, 3),
                    "-",
                    s.Substring(3, 3),
                    "-",
                    s.Substring(6, 6)
                });
            }
            return "";
        }

        public static string GetRandomPassword(int length)
        {
            byte[] data = new byte[length];
            RNGCryptoServiceProvider rngCrypt = new RNGCryptoServiceProvider();
            rngCrypt.GetBytes(data);
            return Convert.ToBase64String(data);
        }

        public static string GetFormatedInsurerINN()
        {
            return DataClass.SelectedInsurer != null ? $"[{ParseRegNum((long)DataClass.SelectedInsurer.INN)}]" : "[___-___-______]";
        }

        public static string GetInsurerINN()
        {
            return DataClass.SelectedInsurer != null ? DataClass.SelectedInsurer.INN.ToString() : "";
        }

        public static string GetInsurerRegNumber()
        {
            return DataClass.SelectedInsurer != null ? DataClass.SelectedInsurer.RegNumber.ToString() : "";
        }

        public static string GetInsurerShortName()
        {
            return DataClass.SelectedInsurer != null ? DataClass.SelectedInsurer.NameShort : " ";
        }
    }
}
