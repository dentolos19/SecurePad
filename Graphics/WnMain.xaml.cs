using System.Windows;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using SecurePad.Core.Models;

namespace SecurePad.Graphics
{

    public partial class WnMain
    {

        private Package _current;
        private string _location;

        public WnMain()
        {
            InitializeComponent();
        }

        private void New(object sender, RoutedEventArgs e)
        {
            _current = null;
            _location = string.Empty;
            Document.Text = string.Empty;
        }

        private void Open(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog()
            {
                Title = "SecurePad File Opener",
                Filter = "SecurePad Encrypted File|*.spef"
            };
            if (openDialog.ShowDialog() == false)
                return;
            var document = Package.Load(openDialog.FileName);
            var password = Interaction.InputBox("Enter the password for this document.", "SecurePad Password Manager");
            if (document.Verify(password, App.Settings.Seed))
            {
                _current = document;
                _location = openDialog.FileName;
                Document.Text = _current.Content;
            }
            else
            {
                MessageBox.Show("Password or security seed is incorrect, access is denied!", "SecurePad Password Manager");
            }
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_location))
            {
                SaveAs(null, null);
            }
            else
            {
                _current.Content = Document.Text;
                _current.Save(_location);
            }
        }

        private void SaveAs(object sender, RoutedEventArgs e)
        {
            var saveDialog = new SaveFileDialog()
            {
                Title = "SecurePad File Saver",
                Filter = "SecurePad Encrypted File|*.spef"
            };
            if (saveDialog.ShowDialog() == false)
                return;
            var password = Interaction.InputBox("Enter new password for this document.", "SecurePad Password Manager");
            _location = saveDialog.FileName;
            _current = new Package(password, App.Settings.Seed)
            {
                Content = Document.Text
            };
            _current.Save(_location);
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Undo(object sender, RoutedEventArgs e)
        {
            if (Document.CanUndo)
                Document.Undo();
        }

        private void Redo(object sender, RoutedEventArgs e)
        {
            if (Document.CanRedo)
                Document.Redo();
        }

        private void Cut(object sender, RoutedEventArgs e)
        {
            Document.Cut();
        }

        private void Copy(object sender, RoutedEventArgs e)
        {
            Document.Copy();
        }

        private void Paste(object sender, RoutedEventArgs e)
        {
            Document.Paste();
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            Document.Delete();
        }

        private void SelectAll(object sender, RoutedEventArgs e)
        {
            Document.SelectAll();
        }

        private void UpdateSecuritySeed(object sender, RoutedEventArgs e)
        {
            var seed = Interaction.InputBox("The file will only open if the password and the security seed are correct.\n\nChange this to add extra layer of protection.", "SecurePad Security Manager", App.Settings.Seed);
            if (string.IsNullOrEmpty(seed) || seed == App.Settings.Seed)
                return;
            App.Settings.Seed = seed;
            App.Settings.Save();
        }

        private void OpenAbout(object sender, RoutedEventArgs e)
        {
            new WnAbout().ShowDialog();
        }

    }

}