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

        private static WnMain _windowMain;

        private void Initialize(object sender, StartupEventArgs e)
        {
            var uri = $"pack://application:,,,/MahApps.Metro;component/Styles/Themes/Light.{Settings.Accent}.xaml";
            if (Settings.IsDarkMode)
                uri = $"pack://application:,,,/MahApps.Metro;component/Styles/Themes/Dark.{Settings.Accent}.xaml";
            var theme = new ResourceDictionary {Source = new Uri(uri)};
            Current.Resources.MergedDictionaries.Add(theme);
            switch (e.Args.Length)
            {
                case 2:
                    _windowMain = new WnMain(e.Args[0], e.Args[1]);
                    break;
                case 1:
                    _windowMain = new WnMain(e.Args[0]);
                    break;
                default:
                    _windowMain = new WnMain();
                    break;
            }
            _windowMain.Show();
        }

        private async void HandleExceptions(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            if (_windowMain.IsLoaded)
            {
                var result = await _windowMain.ShowMessageAsync("SecurePad Code Handler", "Internal code error detected, do you want to continue using SecurePad?", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings {AffirmativeButtonText = "Yes", NegativeButtonText = "No"});
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