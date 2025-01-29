using System.Net.Http;
using System.Net.Http.Json;

namespace DesktopApp
{
    public class Connection
    {
        private static Connection? _instance;
        private readonly HttpClient _client;

        private Connection()
        {
            _client = new HttpClient();
        }

        ~Connection()
        {
            _client.Dispose();
        }

        /// <summary>
        /// Singleton pattern to get the connection instance
        /// </summary>
        /// <returns>Conection instance</returns>
        public static Connection GetConection()
        {
            _instance ??= new Connection();

            return _instance;
        }

        /// <summary>
        /// Sends a request to the server
        /// </summary>
        /// <exception cref="Exception">Throws an exception if the request was not successful</exception>
        public async Task SendAsync(string url, HttpMethod method, StringContent? content = null)
        {
            try
            {
                var response = await _client.SendAsync(new HttpRequestMessage(method, url)
                {
                    Content = content
                });
                if (response.IsSuccessStatusCode)
                {
                    return;
                }
                else
                {
                    throw new Exception(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Sends a request to the server
        /// </summary>
        /// <typeparam name="T">Type of the result from json</typeparam>
        /// <exception cref="Exception">Throws an exception if the request was not successful</exception>
        public async Task<T?> SendAsync<T>(string url, HttpMethod method, StringContent? content = null)
        {
            try
            {
                var response = await _client.SendAsync(new HttpRequestMessage(method, url)
                {
                    Content = content
                });
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<T>();
                }
                else
                {
                    throw new Exception(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
