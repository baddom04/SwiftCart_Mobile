using ShoppingList.Core;
using ShoppingList.Core.Enums;

namespace ShoppingList.Persistor.Services.Interfaces
{
    public interface IGroceryService
    {
        Task CreateGroceryAsync(int household_id, string name, int? quantity, UnitType? unit, string? description, CancellationToken cancellationToken = default);
        Task UpdateGroceryAsync(int household_id, int grocery_id, string? name = null, int? quantity = null, UnitType? unit = null, string? description = null, CancellationToken cancellationToken = default);
        Task<IEnumerable<Grocery>> GetGroceriesAsync(int household_id, CancellationToken cancellationToken = default);
        Task DeleteGroceryAsync(int household_id, int grocery_id, CancellationToken cancellationToken = default);
    }
}
