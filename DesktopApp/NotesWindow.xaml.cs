using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Net.Http.Json;
using System.Diagnostics;

namespace DesktopApp
{
    /// <summary>
    /// Interakční logika pro Notes.xaml
    /// </summary>
    public partial class NotesWindow : Window
    {
        public string Username { get; set; }
        public HttpClient Client { get; set; }
        public class Note(string username)
        {
            public long? IdNote { get; set; }
            public string Content { get; set; }
            public DateTime Date { get; set; } = DateTime.Now;
            public string Username { get; set; } = username;
        }

        public ObservableCollection<Note> Notes { get; set; }
        public NotesWindow(HttpClient client, string Username)
        {
            InitializeComponent();
            this.DataContext = this;
            this.Username = Username;
            this.Client = client;
            this.Notes = new ObservableCollection<Note>();
            LoadNotesAsync();
        }

        ~NotesWindow()
        {
            Client.Dispose();
        }

        private async void LoadNotesAsync()
        {
            try
            {
                var response = await Client.GetAsync("https://localhost:7020/Notes/");

                if (response.IsSuccessStatusCode)
                {
                    Notes.Clear();
                    var notes = await response.Content.ReadFromJsonAsync<List<Note>>() ?? [];
                    foreach (var note in notes)
                    {
                        Notes.Add(note);
                    }
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NewNote(object sender, RoutedEventArgs e)
        {
            var noteWindow = new NoteWindow(new Note(this.Username));
            noteWindow.OnSave += async note => {
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                    };

                    var response = await Client.PostAsync("https://localhost:7020/Notes/", new StringContent(
                        JsonSerializer.Serialize(note, options), Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        LoadNotesAsync();
                    }
                    else
                    {
                        var message = await response.Content.ReadAsStringAsync();
                        MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };
            noteWindow.Show();
        }

        private void EditNote(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Note note = btn.DataContext as Note;
            var noteWindow = new NoteWindow(note);
            noteWindow.OnSave += async note => {
                try
                {
                    var response = await Client.PutAsync($"https://localhost:7020/Notes/{note.IdNote}", new StringContent(
                        JsonSerializer.Serialize(note), Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        LoadNotesAsync();
                    }
                    else
                    {
                        var message = await response.Content.ReadAsStringAsync();
                        MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };
            noteWindow.Show();
        }

        private async void DeleteNote(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                Note note = btn.DataContext as Note;
                this.Notes.Remove(note);

                var response = await Client.DeleteAsync($"https://localhost:7020/Notes/{note.IdNote}");
                if (response.IsSuccessStatusCode)
                {
                    LoadNotesAsync();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
