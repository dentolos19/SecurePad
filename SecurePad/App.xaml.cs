using System.Windows;
using SecurePad.Core;
using SecurePad.Graphics;

namespace SecurePad
{

    public partial class App
    {

        internal static Configuration Settings { get; private set; }

        private void Initialize(object sender, StartupEventArgs args)
        {
            Settings = Configuration.Load();
            Utilities.SetAppTheme(Settings.ThemeAccent, Settings.EnableDarkMode);
            (args.Args.Length > 0 ? new WnMain(args.Args) : new WnMain()).Show();
        }

    }

}