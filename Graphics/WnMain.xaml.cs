using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using SecurePad.Core;
using SecurePad.Core.Models;

namespace SecurePad.Graphics
{

    public partial class WnMain
    {

        private Package _current;

        private string _location;

        private readonly string _prelocation;

        private string _prepassword;

        public WnMain(string location = null, string password = null)
        {
            InitializeComponent();
            if (App.Settings.IsDarkMode)
            {
                Document.Foreground = Brushes.White;
                MenuThemeSwitchItem.Header += "Light Mode";
            }
            else
            {
                MenuThemeSwitchItem.Header += "Dark Mode";
            }
            MenuAccentComboBox.Text = App.Settings.Accent;
            if (!string.IsNullOrEmpty(location))
                _prelocation = location;
            if (!string.IsNullOrEmpty(password))
                _prepassword = password;
        }

        private async void InitializePreLoaded(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_prelocation))
                return;
            var document = Package.Load(_prelocation);
            if (string.IsNullOrEmpty(_prepassword))
                _prepassword = await this.ShowInputAsync("SecurePad Password Manager", "Enter the password for this document.");
            if (document.Verify(_prepassword, App.Settings.Seed))
            {
                _current = document;
                _location = _prelocation;
                Document.Text = _current.Content;
                Document.IsModified = false;
            }
            else
            {
                MessageBox.Show("Either password or security seed is wrong, access is denied!", "SecurePad Password Manager");
            }
        }

        private async void New(object sender, RoutedEventArgs e)
        {
            if (Document.IsModified)
            {
                var result = await this.ShowMessageAsync("SecurePad File Safety", "You have unsaved work, would you like to save the current one?", MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, new MetroDialogSettings
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                    FirstAuxiliaryButtonText = "Cancel"
                });
                if (result == MessageDialogResult.FirstAuxiliary)
                    return;
                if (result == MessageDialogResult.Affirmative)
                    Save(null, null);
            }
            _current = null;
            _location = string.Empty;
            Document.Text = string.Empty;
            Document.IsModified = false;
        }

        private async void Open(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog
            {
                Title = "SecurePad File Opener",
                Filter = "SecurePad Encrypted File|*.spef"
            };
            if (openDialog.ShowDialog() == false)
                return;
            var document = Package.Load(openDialog.FileName);
            var password = await this.ShowInputAsync("SecurePad Password Manager", "Enter the password for this document.");
            if (document.Verify(password, App.Settings.Seed))
            {
                _current = document;
                _location = openDialog.FileName;
                Document.Text = _current.Content;
                Document.IsModified = false;
            }
            else
            {
                await this.ShowMessageAsync("SecurePad Password Manager", "Either password or security seed is wrong, access is denied!");
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
                Document.IsModified = false;
            }
        }

        private async void SaveAs(object sender, RoutedEventArgs e)
        {
            var saveDialog = new SaveFileDialog
            {
                Title = "SecurePad File Saver",
                Filter = "SecurePad Encrypted File|*.spef"
            };
            if (saveDialog.ShowDialog() == false)
                return;
            var currentPassword = string.Empty;
            if (_current != null)
                currentPassword = _current.Password;
            var password = await this.ShowInputAsync("SecurePad Password Manager", "Enter a new password for this document.", new MetroDialogSettings
            {
                DefaultText = currentPassword
            });
            _location = saveDialog.FileName;
            _current = new Package
            {
                Password = password,
                Seed = App.Settings.Seed,
                Content = Document.Text
            };
            _current.Save(_location);
            Document.IsModified = false;
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

        private async void UpdateSecuritySeed(object sender, RoutedEventArgs e)
        {
            var seed = await this.ShowInputAsync("SecurePad Security Manager", "The file will only open if the password and the security seed are correct.\n\nThis seed is generated uniquely for you, change this to update protection.", new MetroDialogSettings
            {
                DefaultText = App.Settings.Seed
            });
            if (string.IsNullOrEmpty(seed) || seed == App.Settings.Seed)
                return;
            App.Settings.Seed = seed;
            App.Settings.Save();
        }

        private void OpenAbout(object sender, RoutedEventArgs e)
        {
            new WnAbout().Show();
        }

        private async void FileDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return;
            if (e.Data.GetData(DataFormats.FileDrop) is string[] data && data.Length == 1)
            {
                var document = Package.Load(data[0]);
                var password = await this.ShowInputAsync("SecurePad Password Manager", "Enter the password for this document.");
                if (document.Verify(password, App.Settings.Seed))
                {
                    _current = document;
                    _location = data[0];
                    Document.Text = _current.Content;
                    Document.IsModified = false;
                }
                else
                {
                    await this.ShowMessageAsync("SecurePad Password Manager", "Either password or security seed is wrong, access is denied!");
                }
            }
            else
            {
                await this.ShowMessageAsync("SecurePad File Dropper", "You can only drop one file at a time!");
            }
        }

        private void CheckUnsaved(object sender, CancelEventArgs e)
        {
            if (!Document.IsModified)
                return;
            var result = MessageBox.Show("You have unsaved work, would you like to save the current one?", "SecurePad File Safety", MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Cancel)
                e.Cancel = true;
            if (result == MessageBoxResult.Yes)
                Save(null, null);
        }

        private async void CheckForUpdates(object sender, RoutedEventArgs e)
        {
            if (Utilities.IsUserOnline())
            {
                if (!Utilities.IsUpdateAvailable())
                {
                    var result = await this.ShowMessageAsync("SecurePad Update Checker", "Updates are available! Do you want to go visit the download page?", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings
                    {
                        AffirmativeButtonText = "Yes",
                        NegativeButtonText = "No"
                    });
                    if (result == MessageDialogResult.Affirmative)
                        Process.Start("https://github.com/dentolos19/SecurePad/releases");
                }
                else
                {
                    await this.ShowMessageAsync("SecurePad Update Checker", "No updates are available, keep doing your thing!");
                }
            }
            else
            {
                await this.ShowMessageAsync("SecurePad Update Checker", "An internet connection is required to perform this operation!");
            }
        }

        private async void SwitchThemeMode(object sender, RoutedEventArgs e)
        {
            if (App.Settings.IsDarkMode)
            {
                App.Settings.IsDarkMode = false;
                MenuThemeSwitchItem.Header = "Switch To Dark Mode";
            }
            else
            {
                App.Settings.IsDarkMode = true;
                MenuThemeSwitchItem.Header = "Switch To Light Mode";
            }
            App.Settings.Save();
            var result = await this.ShowMessageAsync("SecurePad Theme Manager", "Do you want to restart to take effect?", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No"
            });
            if (result != MessageDialogResult.Affirmative)
                return;
            if (string.IsNullOrEmpty(_location) || Document.IsModified)
            {
                var answer = await this.ShowMessageAsync("SecurePad File Safety", "You have unsaved work, would you like to save the current one?", MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, new MetroDialogSettings
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                    FirstAuxiliaryButtonText = "Cancel"
                });
                if (answer == MessageDialogResult.FirstAuxiliary)
                    return;
                if (answer == MessageDialogResult.Affirmative)
                    Save(null, null);
                Utilities.Restart();
                return;
            }
            Utilities.Restart("\"" + _location + "\" \"" + _current.Password + "\"");
        }

        private async void SwitchAccentStyle(object sender, EventArgs e)
        {
            if (MenuAccentComboBox.Text != App.Settings.Accent)
            {
                App.Settings.Accent = MenuAccentComboBox.Text;
                App.Settings.Save();
                var result = await this.ShowMessageAsync("SecurePad Theme Manager", "Do you want to restart to take effect?", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No"
                });
                if (result != MessageDialogResult.Affirmative)
                    return;
                if (string.IsNullOrEmpty(_location) || Document.IsModified)
                {
                    var answer = await this.ShowMessageAsync("SecurePad File Safety", "You have unsaved work, would you like to save the current one?", MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, new MetroDialogSettings
                    {
                        AffirmativeButtonText = "Yes",
                        NegativeButtonText = "No",
                        FirstAuxiliaryButtonText = "Cancel"
                    });
                    if (answer == MessageDialogResult.FirstAuxiliary)
                        return;
                    if (answer == MessageDialogResult.Affirmative)
                        Save(null, null);
                    Utilities.Restart();
                    return;
                }
                Utilities.Restart("\"" + _location + "\" \"" + _current.Password + "\"");
            }
        }

    }

}