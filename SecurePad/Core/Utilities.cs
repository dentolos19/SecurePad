using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
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

        public static void RestartApp(string args = null)
        {
            var location = Assembly.GetExecutingAssembly().Location;
            if (location.EndsWith(".dll", StringComparison.CurrentCultureIgnoreCase))
                location = Path.Combine(Path.GetDirectoryName(location)!, Path.GetFileNameWithoutExtension(location) + ".exe");
            Process.Start(location, args ?? string.Empty);
            Application.Current.Shutdown();
        }

    }

}