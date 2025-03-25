using ShoppingList.Core;
using ShoppingList.Core.Enums;
using ShoppingList.Persistor.DTO;
using ShoppingList.Persistor.Services.Interfaces;
using System.Net.Http.Json;

namespace ShoppingList.Persistor.Services
{
    internal class GroceryService(HttpClient client) : APIService(client), IGroceryService
    {
        public async Task CreateGroceryAsync(int household_id, string name, int? quantity, UnitType? unit, string? description, CancellationToken cancellationToken = default)
        {
            var payload = new { name, quantity, unit = unit?.ToString(), description };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"households/{household_id}/groceries", payload, cancellationToken);

            await ValidateResponse(response, cancellationToken);
        }

        public async Task DeleteGroceryAsync(int household_id, int grocery_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"households/{household_id}/groceries/{grocery_id}", cancellationToken);

            await ValidateResponse(response, cancellationToken);
        }

        public async Task<IEnumerable<Grocery>> GetGroceriesAsync(int household_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"households/{household_id}/groceries", cancellationToken);

            await ValidateResponse(response, cancellationToken);

            GroceriesResponse? groceries = await response.Content.ReadFromJsonAsync<GroceriesResponse>(cancellationToken);

            return groceries?.QueryResult ?? throw new NullReferenceException(nameof(groceries)); ;
        }

        public async Task UpdateGroceryAsync(int household_id, int grocery_id, string? name = null, int? quantity = null, UnitType? unit = null, string? description = null, CancellationToken cancellationToken = default)
        {
            var payload = new { name, quantity, unit, description };

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"households/{household_id}/groceries/{grocery_id}", payload, cancellationToken);

            await ValidateResponse(response, cancellationToken);
        }
    }
}
