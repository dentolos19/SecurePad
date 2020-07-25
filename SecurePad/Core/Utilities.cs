using System;
using System.Windows;

namespace SecurePad.Core
{

    public static class Utilities
    {

        public static void SetAppTheme(string accent, bool setDark)
        {
            var dictionary = new ResourceDictionary
            {
                Source = setDark
                    ? new Uri($"pack://application:,,,/MahApps.Metro;component/Styles/Themes/Dark.{accent}.xaml")
                    : new Uri($"pack://application:,,,/MahApps.Metro;component/Styles/Themes/Light.{accent}.xaml")
            };
            Application.Current.Resources.MergedDictionaries.Add(dictionary);
        }

    }

}