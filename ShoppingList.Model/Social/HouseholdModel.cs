using Microsoft.Extensions.DependencyInjection;
using ShoppingList.Core;
using ShoppingList.Persistor;
using ShoppingList.Persistor.Services.Interfaces;

namespace ShoppingList.Model.Social
{
    public class HouseholdModel(Household household)
    {
        public Household Household { get; } = household;

        private readonly IHouseholdService _householdService = AppServiceProvider.Services.GetRequiredService<IHouseholdService>();
        private readonly IApplicationService _applicationService = AppServiceProvider.Services.GetRequiredService<IApplicationService>();

        public async Task<IEnumerable<User>> GetHouseholdMembersAsync()
        {
            return await _householdService.GetUsersOfHouseholdAsync(Household.Id);
        }

        public async Task<IEnumerable<User>> GetApplicantsAsync()
        {
            return await _applicationService.GetAppliedUsersAsync(Household.Id);
        }

        public async Task DeleteHouseholdAsync()
        {
            await _householdService.DeleteHouseholdAsync(Household.Id);
        }

        public async Task LeaveHousholdAsync(int user_id)
        {
            await _householdService.RemoveMemberAsync(Household.Id, user_id);
        }
    }
}
