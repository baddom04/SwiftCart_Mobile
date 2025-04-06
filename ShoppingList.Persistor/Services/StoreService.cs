using ShoppingList.Core;
using ShoppingList.Persistor.DTO;
using ShoppingList.Persistor.Services.Interfaces;
using System.Net.Http.Json;

namespace ShoppingList.Persistor.Services
{
    internal class StoreService(HttpClient client) : APIService(client), IStoreService
    {
        public async Task<Store> CreateStoreAsync(string name, CancellationToken cancellationToken = default)
        {
            var payload = new { name };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("stores", payload, cancellationToken);

            await ValidateResponse(response, cancellationToken);

            Store? store = await response.Content.ReadFromJsonAsync<Store>(cancellationToken);

            return store ?? throw new NullReferenceException();
        }

        public async Task DeleteStoreAsync(int store_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"stores/{store_id}", cancellationToken);

            await ValidateResponse(response, cancellationToken);
        }

        public async Task<Store?> GetMyStoreAsync(CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.GetAsync("stores/my", cancellationToken);

            await ValidateResponse(response, cancellationToken);

            if(response.StatusCode == System.Net.HttpStatusCode.NoContent)
                return null;

            return await response.Content.ReadFromJsonAsync<Store?>(cancellationToken);
        }

        public async Task<Store> GetStoreAsync(int store_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"stores/{store_id}", cancellationToken);

            await ValidateResponse(response, cancellationToken);

            Store? store = await response.Content.ReadFromJsonAsync<Store>(cancellationToken);

            return store ?? throw new NullReferenceException();
        }

        public async Task<PaginatedResponse<Store>> GetStoresAsync(string search, int page, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"stores/search?search={Uri.EscapeDataString(search)}&per_page={NetworkSettings.StoresPerPage}&page={page}", cancellationToken);

            await ValidateResponse(response, cancellationToken);

            PaginatedResponse<Store>? stores = await response.Content.ReadFromJsonAsync<PaginatedResponse<Store>>(cancellationToken);

            return stores ?? throw new NullReferenceException();
        }

        public async Task<Store> UpdateStoreAsync(int store_id, string name, bool published, CancellationToken cancellationToken = default)
        {
            var payload = new { name, published };

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"stores/{store_id}", payload, cancellationToken);

            await ValidateResponse(response, cancellationToken);

            Store? store = await response.Content.ReadFromJsonAsync<Store>(cancellationToken);

            return store ?? throw new NullReferenceException();
        }
    }
}
