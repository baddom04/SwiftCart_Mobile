using Microsoft.Extensions.DependencyInjection;
using ShoppingList.Core;
using ShoppingList.Persistor;
using ShoppingList.Persistor.Services.Interfaces;

namespace ShoppingList.Model.Social
{
    public class UserListItemModel(User user)
    {
        public User User { get; } = user;

        private readonly IHouseholdService _householdService = AppServiceProvider.Services.GetRequiredService<IHouseholdService>();
        private readonly IApplicationService _applicationService = AppServiceProvider.Services.GetRequiredService<IApplicationService>();

        public async Task AcceptUserAsync(int household_id)
        {
            Application application = await _applicationService.GetApplicationByDataAsync(household_id, User.Id);
            await _applicationService.AcceptUserAsync(application.Id);
        }

        public async Task RemoveMemberAsync(int household_id)
        {
            await _householdService.RemoveMemberAsync(household_id, User.Id);
        }
    }
}
