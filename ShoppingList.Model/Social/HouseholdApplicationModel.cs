using Microsoft.Extensions.DependencyInjection;
using ShoppingList.Core;
using ShoppingList.Persistor;
using ShoppingList.Persistor.Services.Interfaces;

namespace ShoppingList.Model.Social
{
    public class HouseholdApplicationModel(Household household)
    {
        public Household Household { get; } = household;

        private readonly IApplicationService _applicationService = AppServiceProvider.Services.GetRequiredService<IApplicationService>();

        public async Task DeleteApplicationAsync(int household_id, int user_id)
        {
            Application application = await _applicationService.GetApplicationByDataAsync(household_id, user_id);

            await _applicationService.DeleteApplicationAsync(application.Id);
        }
    }
}
