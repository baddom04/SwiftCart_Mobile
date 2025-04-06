using ShoppingList.Core;
using ShoppingList.Persistor.Services.Interfaces;
using System.Net.Http.Json;

namespace ShoppingList.Persistor.Services
{
    internal class LocationService(HttpClient client) : APIService(client), ILocationService
    {
        public async Task<Location> CreateLocationAsync(int store_id, string country, string zip_code, string city, string street, string? detail, CancellationToken cancellationToken = default)
        {
            var payload = new { country, zip_code, city, street, detail };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"stores/{store_id}/location", payload, cancellationToken);

            await ValidateResponse(response, cancellationToken);

            Location? location = await response.Content.ReadFromJsonAsync<Location>(cancellationToken);

            return location ?? throw new NullReferenceException();
        }

        public async Task DeleteLocationAsync(int store_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"stores/{store_id}/location", cancellationToken);

            await ValidateResponse(response, cancellationToken);
        }

        public async Task<Location> GetLocationAsync(int store_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"stores/{store_id}/location", cancellationToken);

            await ValidateResponse(response, cancellationToken);

            Location? location = await response.Content.ReadFromJsonAsync<Location>(cancellationToken);

            return location ?? throw new NullReferenceException();
        }

        public async Task<Location> UpdateLocationAsync(int store_id, string country, string zip_code, string city, string street, string? detail, CancellationToken cancellationToken = default)
        {
            var payload = new { country, zip_code, city, street, detail };

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"stores/{store_id}/location", payload, cancellationToken);

            await ValidateResponse(response, cancellationToken);

            Location? location = await response.Content.ReadFromJsonAsync<Location>(cancellationToken);

            return location ?? throw new NullReferenceException();
        }
    }
}
