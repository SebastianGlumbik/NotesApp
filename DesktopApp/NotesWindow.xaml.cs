using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace DesktopApp
{
    public partial class NotesWindow : Window
    {
        public string Username { get; set; }
        public ObservableCollection<Note> Notes { get; set; }
        public NotesWindow(string Username)
        {
            InitializeComponent();
            this.DataContext = this;
            this.Username = Username;
            this.Notes = new ObservableCollection<Note>();
            LoadNotesAsync();
        }

        private async void LoadNotesAsync()
        {
            try
            {
                var notes = await Note.GetNotesAsync();
                Notes.Clear();
                foreach (var note in notes ?? [])
                {
                    Notes.Add(note);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void NewNote(object sender, RoutedEventArgs e)
        {
            var noteWindow = new NoteWindow(new Note(this.Username));
            noteWindow.OnSave += async note => {
                try
                {
                    await note.AddNoteAsync();
                    LoadNotesAsync();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };
            noteWindow.Show();
        }

        private void EditNote(object sender, RoutedEventArgs e)
        {
            Button? btn = sender as Button;
            Note note = btn?.DataContext as Note ?? new Note(this.Username);
            var noteWindow = new NoteWindow(new Note(this.Username)
            {
                IdNote = note.IdNote,
                Content = note.Content,
                Date = note.Date
            });
            noteWindow.OnSave += async note => {
                try
                {
                    await note.UpdateNoteAsync();
                    LoadNotesAsync();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };
            noteWindow.Show();
        }

        private async void DeleteNote(object sender, RoutedEventArgs e)
        {
            Button? btn = sender as Button;
            Note? note = btn?.DataContext as Note;
            if (note == null)
            {
                return;
            }

            this.Notes.Remove(note);
            try
            {
                await note.DeleteNoteAsync();
                LoadNotesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Logout(object sender, RoutedEventArgs e)
        {
            try
            {
                await User.LogoutAsync();
                new MainWindow().Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
