using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace DesktopApp
{
    public class User()
    {
        /// <summary>
        /// Sends a request to the server to login a user
        /// </summary>
        /// <exception cref="Exception"></exception>
        public async static Task LoginAsync(string username, string password)
        {
            try
            {
                await Connection.GetConection().SendAsync("https://localhost:7020/login", HttpMethod.Post, new StringContent(
                JsonSerializer.Serialize(new
                {
                    username,
                    password
                }), Encoding.UTF8, "application/json"));
                return;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Sends a request to the server to register a new user
        /// </summary>
        /// <exception cref="Exception"></exception>
        public async static Task RegisterAsync(string username, string password)
        {
            try
            {
                await Connection.GetConection().SendAsync("https://localhost:7020/register", HttpMethod.Post, new StringContent(
                JsonSerializer.Serialize(new
                {
                    username,
                    password
                }), Encoding.UTF8, "application/json"));
                return;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Logs out the user
        /// </summary>
        /// <exception cref="Exception"></exception>
        public async static Task LogoutAsync()
        {
            try
            {
                await Connection.GetConection().SendAsync("https://localhost:7020/logout", HttpMethod.Post);
                return;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
