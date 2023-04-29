using System;
using System.Linq;
using System.Windows;

namespace source.Core
{
    static internal class ThemeChanger
    {
        public static void ChangeTheme()
        {
            DataClass.UserInfo.IsUsingDarkMode = !DataClass.UserInfo.IsUsingDarkMode;
            Application.Current.Resources.MergedDictionaries.Clear();
            SetTheme(DataClass.UserInfo.IsUsingDarkMode ?? false);
            DataClass.DataBase.SaveChanges();
        }

        public static void SetTheme(bool enableDarkTheme)
        {
            if (enableDarkTheme) SetDarkTheme();
            else SetLightTheme();
        }

        public static void SetDarkTheme()
        {
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
            {
                Source = new Uri("../Themes/DarkTheme.xaml", UriKind.RelativeOrAbsolute)
            });
        }

        public static void SetLightTheme()
        {
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
            {
                Source = new Uri("../Themes/LightTheme.xaml", UriKind.RelativeOrAbsolute)
            });
        }
    }
}
