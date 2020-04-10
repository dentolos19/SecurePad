using System.Windows;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using SecurePad.Core;

namespace SecurePad.Graphics
{

    public partial class WnMain
    {

        private Package Current;
        private string Location;

        public WnMain()
        {
            InitializeComponent();
        }

        private void New(object sender, RoutedEventArgs e)
        {
            Current = null;
            Location = string.Empty;
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
            if (document.Verify(password))
            {
                Current = document;
                Location = openDialog.FileName;
                Document.Text = Current.Content;
            }
            else
            {
                MessageBox.Show("Password is incorrect, access is denied!", "SecurePad Password Manager");
            }
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Location))
            {
                SaveAs(null, null);
            }
            else
            {
                Current.Content = Document.Text;
                Current.Save(Location);
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
            Location = saveDialog.FileName;
            Current = new Package(password)
            {
                Content = Document.Text
            };
            Current.Save(Location);
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

    }

}