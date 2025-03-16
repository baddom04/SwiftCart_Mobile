using Microsoft.Extensions.DependencyInjection;
using ShoppingList.Core;
using ShoppingList.Persistor;
using ShoppingList.Persistor.Services.Interfaces;

namespace ShoppingList.Model.ShoppingList
{
    public class ShoppingListModel(int householdId)
    {
        private readonly IGroceryService _groceryService = AppServiceProvider.Services.GetRequiredService<IGroceryService>();
        private readonly int _householdId = householdId;
        public async Task<IEnumerable<Grocery>> GetGroceriesAsync()
        {
            return await _groceryService.GetGroceriesAsync(_householdId);
        }
    }
}
