using Microsoft.Extensions.DependencyInjection;
using ShoppingList.Core;
using ShoppingList.Persistor;
using ShoppingList.Persistor.Services.Interfaces;

namespace ShoppingList.Model.Social
{
    public class HouseholdApplicationModel
    {
        private readonly IApplicationService _applicationService;
        public Household Household { get; }
        public HouseholdApplicationModel(Household household)
        {
            _applicationService = AppServiceProvider.Services.GetRequiredService<IApplicationService>();
            Household = household;
        }
        public async Task DeleteApplicationAsync(int household_id, int user_id)
        {
            Application application = await _applicationService.GetApplicationByDataAsync(household_id, user_id);

            await _applicationService.DeleteApplicationAsync(application.Id);
        }
    }
}
