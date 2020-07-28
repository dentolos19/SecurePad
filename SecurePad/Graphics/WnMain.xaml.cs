using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;

namespace SecurePad.Graphics
{

    public partial class WnMain
    {

        private string _currentFilePath;

        public WnMain(string preloadFilePath = null)
        {
            _currentFilePath = preloadFilePath;
            InitializeComponent();
            if (App.Settings.EnableDarkMode)
                Document.Foreground = new SolidColorBrush(Colors.White);
            Document.FontSize = App.Settings.EditorFontSize;
            Document.WordWrap = App.Settings.EditorTextWrap;
        }

        private void LoadFileArgs(object sender, RoutedEventArgs args)
        {
            if (string.IsNullOrEmpty(_currentFilePath))
                return;
            Document.Load(_currentFilePath);
            FilePathStatusLabel.Text = _currentFilePath;
        }

        private void CheckUnsaved(object sender, CancelEventArgs args)
        {
            if (!Document.IsModified)
                return;
            var input = MessageBox.Show("You still have unsaved changes! Do you want to save your file changes?", "SecurePad", MessageBoxButton.YesNoCancel);
            if (input == MessageBoxResult.Yes)
                SaveFile(null, null);
            else if (input == MessageBoxResult.Cancel)
                args.Cancel = true;
        }

        private void NewFile(object sender, ExecutedRoutedEventArgs args)
        {
            if (Document.IsModified)
            {
                var input = MessageBox.Show("You still have unsaved changes! Do you want to save your file changes?", "SecurePad", MessageBoxButton.YesNoCancel);
                if (input == MessageBoxResult.Yes)
                    SaveFile(null, null);
                else if (input == MessageBoxResult.Cancel)
                    return;
            }
            _currentFilePath = string.Empty;
            Document.Text = string.Empty;
            Document.IsModified = false;
            FilePathStatusLabel.Text = "This file is unsaved.";
        }

        private void OpenFile(object sender, ExecutedRoutedEventArgs args)
        {
            if (Document.IsModified)
            {
                var input = MessageBox.Show("You still have unsaved changes! Do you want to save your file changes?", "SecurePad", MessageBoxButton.YesNoCancel);
                if (input == MessageBoxResult.Yes)
                    SaveFile(null, null);
                else if (input == MessageBoxResult.Cancel)
                    return;
            }
            var dialog = new OpenFileDialog { Filter = "Text Document|*.txt|All Files|*.*" };
            if (dialog.ShowDialog() == false)
                return;
            _currentFilePath = dialog.FileName;
            Document.Load(_currentFilePath);
            FilePathStatusLabel.Text = _currentFilePath;
        }

        private void OpenFileDrop(object sender, DragEventArgs args)
        {
            if (!args.Data.GetDataPresent(DataFormats.FileDrop))
                return;
            if (Document.IsModified)
            {
                var input = MessageBox.Show("You still have unsaved changes! Do you want to save your file changes?", "SecurePad", MessageBoxButton.YesNoCancel);
                if (input == MessageBoxResult.Yes)
                    SaveFile(null, null);
                else if (input == MessageBoxResult.Cancel)
                    return;
            }
            var filePaths = (string[])args.Data.GetData(DataFormats.FileDrop);
            if (filePaths == null)
                return;
            if (filePaths.Length > 1)
            {
                MessageBox.Show("Only drop one file at a time!", "SecurePad");
                return;
            }
            _currentFilePath = filePaths[0];
            Document.Load(_currentFilePath);
            FilePathStatusLabel.Text = _currentFilePath;
        }

        private void SaveFile(object sender, ExecutedRoutedEventArgs args)
        {
            if (string.IsNullOrEmpty(_currentFilePath))
                SaveFileAs(null, null);
            else
                Document.Save(_currentFilePath);
        }

        private void SaveFileAs(object sender, ExecutedRoutedEventArgs args)
        {
            var dialog = new SaveFileDialog { Filter = "Text Document|*.txt|All Files|*.*" };
            if (dialog.ShowDialog() == false)
                return;
            _currentFilePath = dialog.FileName;
            Document.Save(_currentFilePath);
            FilePathStatusLabel.Text = _currentFilePath;
        }

        private void ExitApp(object sender, RoutedEventArgs args)
        {
            Application.Current.Shutdown();
        }

        private async void EncryptText(object sender, RoutedEventArgs args)
        {
            var password = await this.ShowInputAsync("Enter Password To Encrypt!", "Enter a new password to encrypt this text.");
            // TODO: Encrypt Document's Text
        }

        private async void DecryptText(object sender, RoutedEventArgs args)
        {
            var password = await this.ShowInputAsync("Enter Password To Decrypt!", "Enter the correct password to decrypt this text.");
            // TODO: Decrypt Document's Text
        }

        private void ShowPreferences(object sender, RoutedEventArgs args)
        {
            new WnPreferences { Owner = this }.ShowDialog();
        }

        private async void ShowAbout(object sender, RoutedEventArgs args)
        {
            await this.ShowMessageAsync("About SecurePad", "This program was created by Dennise Catolos.\n\nContact me on Discord, @dentolos19#6996.\nFind me on GitHub, @dentolos19.\nFind me on Twitter, @dentolos19.");
        }

    }

}