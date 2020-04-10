using System.Windows;
using SecurePad.Core;
using SecurePad.Graphics;

namespace SecurePad
{

    public partial class App
    {

        internal static Configuration Settings = Configuration.Load();

        private void Initialize(object sender, StartupEventArgs e)
        {
            new WnMain().Show();
        }

    }

}