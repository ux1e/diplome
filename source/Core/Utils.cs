using Source.Model;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Windows;
using System.Xml.Linq;

namespace Source.Core
{
    static internal class Utils
    {
        public static string GetExePath() => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static string GetReportsPath(string fileName) => GetExePath() + "\\Reports\\" + fileName;

        public static void MakeSZVTDXml(int szvtdID) //[W.I.P]
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
                new XElement(xnamespace2 + "РегНомер", GetInsurerRegNumber(false)),
                new XElement(xnamespace + "НаименованиеОрганизации", orgName),
                new XElement(xnamespace2 + "ИНН", GetInsurerINN())
            });
            XElement xelement2 = new XElement(xnamespace + "Страхователь", new object[]
            {
                new XElement(xnamespace3 + "РегНомер", GetInsurerRegNumber(false)),
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

            XElement xelement7 = new XElement(xnamespace + "СЗВ");

            using (var context = new diplomeEntities())
            {
                var currentStaff = context.FormsSZV_TD_Stuff.Where(q => q.SZV_TD_Id == DataClass.CurrentFormsSZV_TD.Id);

                foreach (var staff in currentStaff)
                {
                    XElement xelement8 = new XElement(xnamespace + "ЗЛ");
                    XElement xelement9 = new XElement(xnamespace2 + "ФИО");

                    if (!string.IsNullOrEmpty(staff.Worker.LastName))
                    {
                        xelement9.Add(new XElement(xnamespace2 + "Фамилия", staff.Worker.LastName.Trim()));
                    }
                    if (!string.IsNullOrEmpty(staff.Worker.FirstName))
                    {
                        xelement9.Add(new XElement(xnamespace2 + "Имя", staff.Worker.FirstName.Trim()));
                    }
                    if (!string.IsNullOrEmpty(staff.Worker.MiddleName))
                    {
                        xelement9.Add(new XElement(xnamespace2 + "Отчество", staff.Worker.MiddleName.Trim()));
                    }

                    xelement8.Add(xelement9);

                    long? insuranceNumber = staff.Worker.InsuranceNumber;
                    xelement8.Add(new XElement(xnamespace2 + "СНИЛС", insuranceNumber));

                    if (staff.Worker.INN != null)
                    {
                        xelement8.Add(new XElement(xnamespace + "ИНН", staff.Worker.INN));
                    }
                    xelement8.Add(xelement9);

                    XElement xelement13 = new XElement(xnamespace + "СЗВ-ТД");

                    xelement7.Add(xelement8);
                }
                xelement4.Add(xelement7);

            }

            XElement xelement25 = new XElement(xnamespace + "СлужебнаяИнформация", new object[]
            {
                new XElement(xnamespace5 + "GUID", GetUUID()),
                new XElement(xnamespace5 + "ДатаВремя", GetDateTime())
            });

            xdocument.Element(xnamespace + "ЭДПФР").Add(xelement3);
            xdocument.Element(xnamespace + "ЭДПФР").Add(xelement25);

            xdocument.Save(fileName);

            MessageBox.Show("Отчёт создан!", "Информация");
        }

        public static void MakeSZVMXml(int szvmID) //[completed]
        {
            try
            {
                DataClass.CurrentFormsSZV_M = DataClass.DataBase.FormsSZV_M.First((FormsSZV_M x) => x.Id == szvmID);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: \n{ex}\nОбратитесь к администратору!", "Произошла ошибка");
                return;
            }

            if (DataClass.CurrentFormsSZV_M == null)
            {
                MessageBox.Show("FormsSZV_M == null", "Ошибка", (MessageBoxButton)MessageBoxImage.Error);
                return;
            }

            string fileName = string.Format("ПФР_СЗВ-М_{0}_{1}.XML", DataClass.UserInfo.Name, GetDateTime());

            XNamespace ns = "http://пф.рф/ВС/СЗВ-М/2017-01-01";
            XNamespace xnamespace = "http://пф.рф/унифицированныеТипы/2014-01-01";
            XNamespace xnamespace2 = "http://пф.рф/АФ";
            XNamespace xnamespace3 = "http://пф.рф/АФ/2017-01-01";
            XNamespace xnamespace4 = "http://www.w3.org/2001/XMLSchema-instance";
            XNamespace xnamespace5 = "http://www.w3.org/2000/09/xmldsig#";

            XDocument xdocument = new XDocument(new XDeclaration("1.0", "UTF-8", null), new object[]
            {
                new XElement(ns + "ЭДПФР", new object[]
                {
                    new XAttribute(XNamespace.Xmlns + "УТ", xnamespace.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "АФ", xnamespace2.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "АФ2", xnamespace3.NamespaceName)
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

            XElement xelement = new XElement(ns + "СЗВ-М", new object[]
            {
                new XElement(ns + "ТипФормы", 1),
                new XElement(ns + "Страхователь", new object[]
                {
                    new XElement(ns + "РегНомер", GetInsurerRegNumber(false)),
                    new XElement(ns + "Наименование", orgName),
                    new XElement(ns + "ИНН", GetInsurerINN())
                }),

                new XElement(ns + "ОтчетныйПериод", new object[]
                {
                    new XElement(xnamespace + "Месяц", DataClass.CurrentFormsSZV_M.Month),
                    new XElement(xnamespace + "КалендарныйГод", DataClass.CurrentFormsSZV_M.Year)
                })
            });

            XElement xelement2 = new XElement(ns + "СписокЗЛ");
            int ppCount = 0;

            using (var context = new diplomeEntities())
            {
                var currentStaff = context.FormsSZV_M_Stuff.Where(q => q.SZV_M_Id == DataClass.CurrentFormsSZV_M.Id);

                foreach (var staff in currentStaff)
                {
                    ppCount++;
                    XElement xelement3 = new XElement(ns + "ЗЛ", new XAttribute("НомерПП", ppCount));
                    XElement xelement4 = new XElement(ns + "ФИО");
                    if (!string.IsNullOrEmpty(staff.Worker.LastName))
                    {
                        xelement4.Add(new XElement(xnamespace + "Фамилия", staff.Worker.LastName.Trim().ToUpper()));
                    }
                    if (!string.IsNullOrEmpty(staff.Worker.FirstName))
                    {
                        xelement4.Add(new XElement(xnamespace + "Имя", staff.Worker.FirstName.Trim().ToUpper()));
                    }
                    if (!string.IsNullOrEmpty(staff.Worker.MiddleName))
                    {
                        xelement4.Add(new XElement(xnamespace + "Отчество", staff.Worker.MiddleName.Trim().ToUpper()));
                    }
                    xelement3.Add(xelement4);

                    if (!string.IsNullOrEmpty(staff.Worker.INN.ToString()) && staff.Worker.INN.ToString() != "0")
                    {
                        xelement3.Add(new XElement(ns + "ИНН", staff.Worker.INN.ToString().PadLeft(12, '0')));
                    }
                    xelement2.Add(xelement3);
                }
                xelement.Add(xelement2);
            }

            xelement.Add(new XElement(ns + "ДатаЗаполнения", DataClass.CurrentFormsSZV_M.DateFilling));
            xdocument.Element(ns + "ЭДПФР").Add(xelement);
            xdocument.Element(ns + "ЭДПФР").Add(new XElement(ns + "СлужебнаяИнформация", new object[]
            {
                new XElement(xnamespace2 + "GUID", GetUUID()),
                new XElement(xnamespace2 + "ДатаВремя", GetDateTime())
            }));

            xdocument.Save(fileName);

            MessageBox.Show("Отчёт создан!", "Информация");
        }

        public static string GetUUID()
        {
            Guid myuuid = Guid.NewGuid();
            return myuuid.ToString();
        }

        public static string GetDateTime() //get normal date format
        {
            DateTime currentDateTime = DateTime.Now;
            return currentDateTime.ToString("MM-dd-yyyy HH.mm.ss");
        }

        public static string ParseNum(long n) //parse reg num to new view
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

        public static string GetFormatedInsurerINN() //get formater inn for insurer [legacy]
        {
            return DataClass.SelectedInsurer != null ? $"[{ParseNum((long)DataClass.SelectedInsurer.INN)}]" : "[___-___-______]";
        }

        public static string GetInsurerINN()
        {
            return DataClass.SelectedInsurer != null ? DataClass.SelectedInsurer.INN.ToString() : "";
        }

        public static string GetInsurerRegNumber(bool isFormated)
        {
            if (isFormated)
            {
                return DataClass.SelectedInsurer != null ? $"{ParseNum((long)DataClass.SelectedInsurer.RegNumber)}" : "";
            }
            else
            {
                return DataClass.SelectedInsurer != null ? DataClass.SelectedInsurer.RegNumber.ToString() : "";
            }
        }

        public static string GetInsurerShortName()
        {
            return DataClass.SelectedInsurer != null ? DataClass.SelectedInsurer.NameShort : " ";
        }
    }
}
