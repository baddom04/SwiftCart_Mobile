using Microsoft.Extensions.DependencyInjection;
using ShoppingList.Core;
using ShoppingList.Persistor;
using ShoppingList.Persistor.Services.Interfaces;

namespace ShoppingList.Model.Social
{
    public class ManageApplicationsModel
    {
        private IEnumerable<Household> _householdsAppliedTo = null!;
        private readonly IApplicationService _applicationService;

        public ManageApplicationsModel()
        {
            _applicationService = AppServiceProvider.Services.GetRequiredService<IApplicationService>();
        }

        public async Task<IEnumerable<Household>> GetAppliedToHouseholdsAsync(int user_id)
        {
            _householdsAppliedTo = await _applicationService.GetAppliedToHouseholdsAsync(user_id);

            return _householdsAppliedTo;
        }
    }
}
