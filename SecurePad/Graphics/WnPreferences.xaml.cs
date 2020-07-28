using System.Windows;
using SecurePad.Core;

namespace SecurePad.Graphics
{

    public partial class WnPreferences
    {

        public WnPreferences()
        {
            InitializeComponent();
        }

        private void LoadSettings(object sender, RoutedEventArgs args)
        {
            ThemeAccentBox.Text = App.Settings.ThemeAccent;
            EditorFontSizeBox.Value = App.Settings.EditorFontSize;
            EditorTextWrapSwitch.IsChecked = App.Settings.EditorTextWrap;
            EnableDarkModeSwitch.IsChecked = App.Settings.EnableDarkMode;
        }

        private void SaveSettings(object sender, RoutedEventArgs args)
        {
            App.Settings.ThemeAccent = ThemeAccentBox.Text;
            App.Settings.EditorFontSize = (int)EditorFontSizeBox.Value!;
            App.Settings.EditorTextWrap = EditorTextWrapSwitch.IsChecked == true;
            App.Settings.EnableDarkMode = EnableDarkModeSwitch.IsChecked == true;
            App.WindowMain.Document.FontSize = App.Settings.EditorFontSize;
            App.WindowMain.Document.WordWrap = App.Settings.EditorTextWrap;
            App.Settings.Save();
            if (MessageBox.Show("All settings has been saved, do you want to restart this program?", "SecurePad", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Utilities.RestartApp();
            }
            else
            {
                Close();
            }
        }

        private void ResetSettings(object sender, RoutedEventArgs args)
        {
            App.Settings.Reset();
            MessageBox.Show("All settings has changed to their default settings, after closing this window this program will restart.", "SecurePad");
            Utilities.RestartApp();
        }

    }

}