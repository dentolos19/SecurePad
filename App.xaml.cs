using System;
using System.Windows;
using SecurePad.Core;
using SecurePad.Graphics;

namespace SecurePad
{

    public partial class App
    {

        internal static readonly Configuration Settings = Configuration.Load();

        private void Initialize(object sender, StartupEventArgs e)
        {
            var uri = $"pack://application:,,,/MahApps.Metro;component/Styles/Themes/Light.{Settings.Accent}.xaml";
            if (Settings.IsDarkMode)
                uri = $"pack://application:,,,/MahApps.Metro;component/Styles/Themes/Dark.{Settings.Accent}.xaml";
            var theme = new ResourceDictionary
            {
                Source = new Uri(uri)
            };
            Current.Resources.MergedDictionaries.Add(theme);
            switch (e.Args.Length)
            {
                case 2:
                    new WnMain(e.Args[0], e.Args[1]).Show();
                    break;
                case 1:
                    new WnMain(e.Args[0]).Show();
                    break;
                default:
                    new WnMain().Show();
                    break;
            }
        }

    }

}