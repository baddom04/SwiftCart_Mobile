using ShoppingList.Core;
using ShoppingList.Persistor.Services.Interfaces;
using System.Net.Http.Json;

namespace ShoppingList.Persistor.Services
{
    internal class StoreService(HttpClient client) : APIService(client), IStoreService
    {
        public async Task CreateStoreAsync(string name, CancellationToken cancellationToken = default)
        {
            var payload = new { name };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("stores", payload, cancellationToken);

            await ValidateResponse(response, cancellationToken);
        }

        public async Task DeleteStoreAsync(int store_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"stores/{store_id}", cancellationToken);

            await ValidateResponse(response, cancellationToken);
        }

        public async Task<Store> GetStoreAsync(int store_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"stores/{store_id}", cancellationToken);

            await ValidateResponse(response, cancellationToken);

            Store? store = await response.Content.ReadFromJsonAsync<Store>(cancellationToken);

            return store ?? throw new NullReferenceException();
        }

        public async Task<IEnumerable<Store>> GetStoresAsync(CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"stores", cancellationToken);

            await ValidateResponse(response, cancellationToken);

            IEnumerable<Store>? stores = await response.Content.ReadFromJsonAsync<IEnumerable<Store>>(cancellationToken);

            return stores ?? throw new NullReferenceException();
        }

        public async Task UpdateStoreAsync(int store_id, string name, CancellationToken cancellationToken = default)
        {
            var payload = new { name };

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"stores/{store_id}", payload, cancellationToken);

            await ValidateResponse(response, cancellationToken);
        }
    }
}
