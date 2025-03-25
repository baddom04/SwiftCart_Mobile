using ShoppingList.Core;
using ShoppingList.Persistor.Services.Interfaces;
using System.Net.Http.Json;

namespace ShoppingList.Persistor.Services
{
    internal class MapService(HttpClient client) : APIService(client), IMapService
    {
        public async Task CreateMapAsync(int store_id, int x_size, int y_size, CancellationToken cancellationToken = default)
        {
            var payload = new { x_size, y_size };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"stores/{store_id}/map", payload, cancellationToken);

            await ValidateResponse(response, cancellationToken);
        }

        public async Task DeleteMapAsync(int store_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"stores/{store_id}/map", cancellationToken);

            await ValidateResponse(response, cancellationToken);
        }

        public async Task<Map> GetMapAsync(int store_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"stores/{store_id}/map", cancellationToken);

            await ValidateResponse(response, cancellationToken);

            Map? map = await response.Content.ReadFromJsonAsync<Map>(cancellationToken);

            return map ?? throw new NullReferenceException();
        }

        public async Task UpdateMapAsync(int store_id, int x_size, int y_size, CancellationToken cancellationToken = default)
        {
            var payload = new { x_size, y_size };

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"stores/{store_id}/map", payload, cancellationToken);

            await ValidateResponse(response, cancellationToken);
        }
    }
}
