using Source.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
        public static User SelectedUser = null;

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

        public static string GetInsurerINN()
        {
            return SelectedInsurer != null ? $"[{ParseRegNum((long)SelectedInsurer.INN)}]" : "[___-___-______]";
        }

        public static string GetInsurerShortName()
        {
            return SelectedInsurer != null ? SelectedInsurer.NameShort : " ";
        }
    }
}
