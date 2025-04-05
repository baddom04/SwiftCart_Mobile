using ShoppingList.Core;
using ShoppingList.Persistor.Services.Interfaces;
using System.Net.Http.Json;

namespace ShoppingList.Persistor.Services
{
    internal class SectionService(HttpClient client) : APIService(client), ISectionService
    {
        public async Task<Section> CreateSectionAsync(int map_id, string name, CancellationToken cancellationToken = default)
        {
            var payload = new { name };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"maps/{map_id}/sections", payload, cancellationToken);

            await ValidateResponse(response, cancellationToken);

            Section? sections = await response.Content.ReadFromJsonAsync<Section>(cancellationToken);

            return sections ?? throw new NullReferenceException();
        }

        public async Task DeleteSectionAsync(int map_id, int section_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"maps/{map_id}/sections/{section_id}", cancellationToken);

            await ValidateResponse(response, cancellationToken);
        }

        public async Task<IEnumerable<Section>> GetSectionAsync(int map_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"maps/{map_id}/sections", cancellationToken);

            await ValidateResponse(response, cancellationToken);

            IEnumerable<Section>? sections = await response.Content.ReadFromJsonAsync<IEnumerable<Section>>(cancellationToken);

            return sections ?? throw new NullReferenceException();
        }

        public async Task<Section> UpdateSectionAsync(int map_id, int section_id, string name, CancellationToken cancellationToken = default)
        {
            var payload = new { name };

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"maps/{map_id}/sections/{section_id}", payload, cancellationToken);

            await ValidateResponse(response, cancellationToken);

            Section? sections = await response.Content.ReadFromJsonAsync<Section>(cancellationToken);

            return sections ?? throw new NullReferenceException();
        }
    }
}
