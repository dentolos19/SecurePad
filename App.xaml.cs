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
            if (e.Args.Length == 1)
                new WnMain(e.Args[0]).Show();
            else
                new WnMain().Show();
        }

    }

}