using System.Windows;
using System.Windows.Media;

namespace SecurePad.Graphics
{

    public partial class WnAbout
    {

        public WnAbout()
        {
            InitializeComponent();
            if (App.Settings.IsDarkMode)
                TopPanel.Background = new BrushConverter().ConvertFrom("#FF444444") as Brush;
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Hide();
        }

    }

}