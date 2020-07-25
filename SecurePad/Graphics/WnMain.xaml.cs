using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using MahApps.Metro.Controls.Dialogs;

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
        }

        private void LoadFileArgs(object sender, RoutedEventArgs args)
        {
            if (string.IsNullOrEmpty(_currentFilePath))
                return;
            // TODO
        }

        private void ClickUnsaved(object sender, CancelEventArgs args)
        {
            // TODO
        }

        private void NewFile(object sender, RoutedEventArgs args)
        {
            // TODO
        }

        private void OpenFile(object sender, RoutedEventArgs args)
        {
            // TODO
        }

        private void SaveFile(object sender, RoutedEventArgs args)
        {
            // TODO
        }

        private void SaveFileAs(object sender, RoutedEventArgs args)
        {
            // TODO
        }

        private void ExitApp(object sender, RoutedEventArgs args)
        {
            Application.Current.Shutdown();
        }

        private void UndoText(object sender, RoutedEventArgs args)
        {
            if (Document.CanUndo)
                Document.Undo();
        }

        private void RedoText(object sender, RoutedEventArgs args)
        {
            if (Document.CanRedo)
                Document.Redo();
        }

        private void CutText(object sender, RoutedEventArgs args)
        {
            Document.Cut();
        }

        private void CopyText(object sender, RoutedEventArgs args)
        {
            Document.Copy();
        }

        private void PasteText(object sender, RoutedEventArgs args)
        {
            Document.Paste();
        }

        private void DeleteText(object sender, RoutedEventArgs args)
        {
            Document.Delete();
        }

        private void SelectAllText(object sender, RoutedEventArgs args)
        {
            Document.SelectAll();
        }

        private void ShowPreferences(object sender, RoutedEventArgs args)
        {
            new WnPreferences { Owner = this }.Show();
        }

        private async void ShowAbout(object sender, RoutedEventArgs args)
        {
            await this.ShowMessageAsync("About SecurePad", "This program was created by Dennise Catolos.\n\nContact me on Discord, @dentolos19#6996.\nFind me on GitHub, @dentolos19.\nFind me on Twitter, @dentolos19.");
        }

    }

}