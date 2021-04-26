using System.Windows;
using System.Windows.Media;
using ControlzEx.Theming;
using SecurePad.Core;

namespace SecurePad.Graphics
{

    public partial class WnPreferences
    {

        public WnPreferences()
        {
            InitializeComponent();
            foreach (var color in ThemeManager.Current.ColorSchemes) ThemeAccentBox.Items.Add(color);
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
            ThemeManager.Current.ChangeThemeColorScheme(Application.Current, App.Settings.ThemeAccent);
            App.WindowMain.Document.FontSize = App.Settings.EditorFontSize;
            App.WindowMain.Document.WordWrap = App.Settings.EditorTextWrap;
            if (App.Settings.EnableDarkMode)
            {
                ThemeManager.Current.ChangeThemeBaseColor(Application.Current, ThemeManager.BaseColorDarkConst);
                App.WindowMain.Document.Foreground = new SolidColorBrush(Colors.White);
            }
            else
            {
                ThemeManager.Current.ChangeThemeBaseColor(Application.Current, ThemeManager.BaseColorLightConst);
                App.WindowMain.Document.Foreground = new SolidColorBrush(Colors.Black);
            }
            App.Settings.Save();
            MessageBox.Show("All settings has been saved!", "SecurePad");
        }

        private void ResetSettings(object sender, RoutedEventArgs args)
        {
            App.Settings.Reset();
            MessageBox.Show("All settings has changed to their default settings, after closing this dialog this program will restart.", "SecurePad");
            Utilities.RestartApp();
        }

    }

}