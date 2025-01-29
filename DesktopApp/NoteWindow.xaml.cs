using System;
using System.Collections.Generic;
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
using static DesktopApp.NotesWindow;

namespace DesktopApp
{
    /// <summary>
    /// Interakční logika pro Note.xaml
    /// </summary>
    public delegate void SaveNote(Note n);
    public partial class NoteWindow : Window
    {
        public Note Note { get; set; }
        public event SaveNote OnSave;
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