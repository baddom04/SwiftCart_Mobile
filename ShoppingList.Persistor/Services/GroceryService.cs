using ShoppingList.Core;
using ShoppingList.Core.Enums;
using ShoppingList.Persistor.Services.Interfaces;
using System.Net.Http.Json;

namespace ShoppingList.Persistor.Services
{
    public class GroceryService(HttpClient client) : APIService(client), IGroceryService
    {
        public async Task CreateGrocery(int household_id, string name, int quantity, UnitType unit, string description, CancellationToken cancellationToken = default)
        {
            var payload = new { name, quantity, unit, description };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"households/{household_id}/groceries", payload, cancellationToken);

            await ValidateResponse(response, cancellationToken);
        }

        public async Task DeleteGrocery(int household_id, int grocery_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"households/{household_id}/groceries/{grocery_id}", cancellationToken);

            await ValidateResponse(response, cancellationToken);
        }

        public async Task<IEnumerable<Grocery>> GetGroceries(int household_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"households/{household_id}/groceries", cancellationToken);

            await ValidateResponse(response, cancellationToken);

            IEnumerable<Grocery>? groceries = await response.Content.ReadFromJsonAsync<IEnumerable<Grocery>>(cancellationToken); 

            return groceries ?? throw new NullReferenceException(nameof(groceries));
        }

        public async Task UpdateGrocery(int household_id, int grocery_id, string? name = null, int? quantity = null, UnitType? unit = null, string? description = null, CancellationToken cancellationToken = default)
        {
            var payload = new { name, quantity, unit, description };

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"households/{household_id}/groceries/{grocery_id}", payload, cancellationToken);

            await ValidateResponse(response, cancellationToken);
        }
    }
}
