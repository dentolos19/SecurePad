using System;
using System.Windows;
using System.Windows.Threading;
using MahApps.Metro.Controls.Dialogs;
using SecurePad.Core;
using SecurePad.Graphics;

namespace SecurePad
{

    public partial class App
    {

        internal static readonly Configuration Settings = Configuration.Load();

        internal static WnMain WindowMain;

        internal static WnAbout WindowAbout;

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
            if (e.Args.Length == 2)
                WindowMain = new WnMain(e.Args[0], e.Args[1]);
            else if (e.Args.Length == 1)
                WindowMain = new WnMain(e.Args[0]);
            else
                WindowMain = new WnMain();
            WindowMain.Show();
        }

        private async void HandleExceptions(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            if (WindowMain.IsLoaded)
            {
                var result = await WindowMain.ShowMessageAsync("SecurePad Code Handler", "Internal code error detected, do you want to continue using SecurePad?", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No"
                });
                if (result == MessageDialogResult.Affirmative)
                    e.Handled = true;
            }
            else
            {
                var result = MessageBox.Show("Internal code error detected, do you want to continue using SecurePad?", "SecurePad Code Handler", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                    e.Handled = true;
            }
        }

    }

}