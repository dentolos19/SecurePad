using System.Windows;
using SecurePad.Core;
using SecurePad.Graphics;

namespace SecurePad
{

    public partial class App
    {

        internal static Configuration Settings { get; private set; }

        internal static WnMain WindowMain { get; private set; }

        private void Initialize(object sender, StartupEventArgs args)
        {
            Settings = Configuration.Load();
            Utilities.SetAppTheme(Settings.ThemeAccent, Settings.EnableDarkMode);
            WindowMain = args.Args.Length == 1 ? new WnMain(args.Args[0]) : new WnMain();
            WindowMain.Show();
        }

    }

}