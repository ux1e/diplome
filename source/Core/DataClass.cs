using Source.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Source.Core
{
    static internal class DataClass
    {
        public static diplomeEntities DataBase = null;
        public static User UserInfo = null;
        public static Insurer SelectedInsurer = null;

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
    }
}
