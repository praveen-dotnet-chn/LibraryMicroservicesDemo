using System.Net.Http;
using UserService.Models;

namespace UserService.Services
{
    public class BookApiClient
    {
        private readonly HttpClient _client;

        public BookApiClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<BookDto?> GetBookByIdAsync(int id)
        {
            try
            {
                return await _client.GetFromJsonAsync<BookDto>($"api/books/{id}");
            }
            catch
            {
                return null;
            }
        }
    }
}
