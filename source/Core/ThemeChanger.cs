using MaterialDesignThemes.Wpf;

namespace Source.Core
{
    static internal class ThemeChanger
    {
        private static readonly PaletteHelper _pHellper = new PaletteHelper();
        private static readonly ITheme _theme = _pHellper.GetTheme();

        public static void ChangeTheme()
        {
            DataClass.UserInfo.IsUsingDarkMode = !DataClass.UserInfo.IsUsingDarkMode;
            SetTheme(DataClass.UserInfo.IsUsingDarkMode ?? false);
            DataClass.DataBase.SaveChanges();
        }

        public static void SetTheme(bool enableDarkTheme)
        {
            if (enableDarkTheme) SetDarkTheme();
            else SetLightTheme();

            _pHellper.SetTheme(_theme);
        }

        public static void SetDarkTheme() => _theme.SetBaseTheme(Theme.Dark);

        public static void SetLightTheme() => _theme.SetBaseTheme(Theme.Light);
    }
}
