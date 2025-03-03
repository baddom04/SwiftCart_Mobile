using Microsoft.Extensions.DependencyInjection;
using ShoppingList.Core.Enums;
using ShoppingList.Persistor;
using ShoppingList.Persistor.Services;

namespace ShoppingList.Model.Models
{
    public class HouseholdListItemModel
    {
        private readonly ApplicationService _applicationService;
        private readonly HouseholdService _householdService;

        public HouseholdListItemModel()
        {
            _applicationService = AppServiceProvider.Services.GetRequiredService<ApplicationService>();
            _householdService = AppServiceProvider.Services.GetRequiredService<HouseholdService>();
        }

        public async Task Apply(int household_id)
        {
            await _applicationService.ApplyAsync(household_id);
        }

        public async Task<HouseholdRelationship> GetHouseholdRelationship(int household_id)
        {
            return await _householdService.GetUserRelationShipAsync(household_id);
        }
    }
}
