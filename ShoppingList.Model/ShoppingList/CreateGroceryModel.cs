using Microsoft.Extensions.DependencyInjection;
using ShoppingList.Core.Enums;
using ShoppingList.Persistor;
using ShoppingList.Persistor.Services.Interfaces;

namespace ShoppingList.Model.ShoppingList
{
    public class CreateGroceryModel
    {
        private readonly IGroceryService _groceryService = AppServiceProvider.Services.GetRequiredService<IGroceryService>();

        public async Task CreateGroceryAsync(int household_id, string name, int? quantity, UnitType? unit, string? description)
        {
            await _groceryService.CreateGroceryAsync(household_id, name, quantity, unit, description);
        }
    }
}
