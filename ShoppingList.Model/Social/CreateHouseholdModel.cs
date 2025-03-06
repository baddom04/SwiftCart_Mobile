using Microsoft.Extensions.DependencyInjection;
using ShoppingList.Persistor;
using ShoppingList.Persistor.Services.Interfaces;

namespace ShoppingList.Model.Social
{
    public class CreateHouseholdModel
    {
        private IHouseholdService _householdService;

        public CreateHouseholdModel()
        {
            _householdService = AppServiceProvider.Services.GetRequiredService<IHouseholdService>();
        }

        public async Task CreateHouseholdAsync(string name, string identifier)
        {
            await _householdService.CreateNewHouseholdAsync(name, identifier);
        }
    }
}
