using System;
using System.Windows;

namespace source.Core
{
    static internal class ThemeChanger
    {
        public static void ChangeTheme()
        {
            DataClass.IsEnabledDarkMode = !DataClass.IsEnabledDarkMode;
            Application.Current.Resources.MergedDictionaries.Clear();

            if (DataClass.IsEnabledDarkMode)
            {
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { 
                    Source = new Uri("../Themes/DarkTheme.xaml", UriKind.RelativeOrAbsolute)
                });
            }
            else
            {
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { 
                    Source = new Uri("../Themes/LightTheme.xaml", UriKind.RelativeOrAbsolute) 
                });
            }
        }
    }
}
