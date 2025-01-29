using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows;

namespace DesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public HttpClient Client { get; set; } = new HttpClient();
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Login(object sender, RoutedEventArgs e)
        {
            try
            {
                var response = await Client.PostAsync("https://localhost:7020/login", new StringContent(
                JsonSerializer.Serialize(new
                {
                    username = loginUsername.Text,
                    password = loginPassword.Password
                }), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    new NotesWindow(Client, loginUsername.Text).Show();
                    this.Close();
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

        private async void Register(object sender, RoutedEventArgs e)
        {
            if (registerPassword1.Password != registerPassword2.Password)
            {
                MessageBox.Show("Passwords do not match", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var response = await Client.PostAsync("https://localhost:7020/register", new StringContent(
                JsonSerializer.Serialize(new
                {
                    username = registerUsername.Text,
                    password = registerPassword1.Password
                }), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("You can now login", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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