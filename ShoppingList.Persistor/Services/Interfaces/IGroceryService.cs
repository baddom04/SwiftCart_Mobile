using ShoppingList.Core;
using ShoppingList.Core.Enums;

namespace ShoppingList.Persistor.Services.Interfaces
{
    internal interface IGroceryService
    {
        Task CreateGrocery(int household_id, string name, int quantity, UnitType unit, string description, CancellationToken cancellationToken = default);
        Task UpdateGrocery(int household_id, int grocery_id, string? name = null, int? quantity = null, UnitType? unit = null, string? description = null, CancellationToken cancellationToken = default);
        Task<IEnumerable<Grocery>> GetGroceries(int household_id, CancellationToken cancellationToken = default);
        Task DeleteGrocery(int household_id, int grocery_id, CancellationToken cancellationToken = default);
    }
}
