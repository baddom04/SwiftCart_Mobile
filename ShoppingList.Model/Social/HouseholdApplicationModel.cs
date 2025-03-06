using Microsoft.Extensions.DependencyInjection;
using ShoppingList.Core;
using ShoppingList.Persistor;
using ShoppingList.Persistor.Services.Interfaces;

namespace ShoppingList.Model.Social
{
    public class HouseholdApplicationModel(Household household)
    {
        private readonly IApplicationService _applicationService = AppServiceProvider.Services.GetRequiredService<IApplicationService>();
        public Household Household { get; } = household;

        public async Task DeleteApplicationAsync(int household_id, int user_id)
        {
            Application application = await _applicationService.GetApplicationByDataAsync(household_id, user_id);

            await _applicationService.DeleteApplicationAsync(application.Id);
        }
    }
}
