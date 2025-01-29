using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace DesktopApp
{
    public class Note(string username)
    {
        public long? IdNote { get; set; }
        public string Content { get; set; } = "";
        public DateTime Date { get; set; } = DateTime.Now;
        public string Username { get; set; } = username;

        /// <summary>
        /// Gets all notes from the server
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static async Task<List<Note>> GetAllAsync()
        {
            try
            {
                var result = await Connection.GetConection().SendAsync<List<Note>>("https://localhost:7020/Notes/", HttpMethod.Get);
                return result ?? [];
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Adds a new note to the server
        /// </summary>
        /// <exception cref="Exception"></exception>
        public async Task AddAsync()
        {
            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            try
            {
                await Connection.GetConection().SendAsync("https://localhost:7020/Notes/", HttpMethod.Post, new StringContent(JsonSerializer.Serialize(this, options), Encoding.UTF8, "application/json"));
                return;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Updates a note on the server
        /// </summary>
        /// <exception cref="Exception"></exception>
        public async Task UpdateAsync()
        {
            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };
            try
            {
                await Connection.GetConection().SendAsync($"https://localhost:7020/Notes/{this.IdNote}", HttpMethod.Put, new StringContent(JsonSerializer.Serialize(this), Encoding.UTF8, "application/json"));
                return;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes a note from the server
        /// </summary>
        /// <exception cref="Exception"></exception>
        public async Task DeleteAsync()
        {
            try
            {
                await Connection.GetConection().SendAsync($"https://localhost:7020/Notes/{this.IdNote}", HttpMethod.Delete);
                return;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
