using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace SecurePad.Graphics
{

    public partial class WnMain
    {

        private readonly string[] _fileArgs;

        public WnMain(string[] fileArgs = null)
        {
            _fileArgs = fileArgs;
            InitializeComponent();
            if (App.Settings.EnableDarkMode)
                Document.Foreground = new SolidColorBrush(Colors.White);
        }

        private void LoadFileArgs(object sender, RoutedEventArgs args)
        {
            if (!(_fileArgs != null && _fileArgs.Length > 0))
                return;
            // TODO
        }

        private void ClickUnsaved(object sender, CancelEventArgs args)
        {
            // TODO
        }

        private void New(object sender, RoutedEventArgs args)
        {
            // TODO
        }

        private void Open(object sender, RoutedEventArgs args)
        {
            // TODO
        }

        private void Save(object sender, RoutedEventArgs args)
        {
            // TODO
        }

        private void SaveAs(object sender, RoutedEventArgs args)
        {
            // TODO
        }

        private void Exit(object sender, RoutedEventArgs args)
        {
            Application.Current.Shutdown();
        }

    }

}