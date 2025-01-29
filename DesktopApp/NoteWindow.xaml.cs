using System.Windows;

namespace DesktopApp
{
    public delegate void SaveNote(Note n);
    public partial class NoteWindow : Window
    {
        public Note Note { get; set; }
        public event SaveNote? OnSave;
        public NoteWindow(Note note)
        {
            InitializeComponent();
            this.Note = note;
            this.DataContext = this.Note;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            this.OnSave?.Invoke(this.Note);
            this.Close();
        }
    }
}