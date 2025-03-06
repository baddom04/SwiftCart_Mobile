using Microsoft.Extensions.DependencyInjection;
using ShoppingList.Core;
using ShoppingList.Persistor;
using ShoppingList.Persistor.Services.Interfaces;

namespace ShoppingList.Model.Social
{
    public class ManageHouseholdsModel
    {
        private readonly IHouseholdService _householdService;
        private IEnumerable<Household> _myHouseholds;
        public ManageHouseholdsModel()
        {
            _householdService = AppServiceProvider.Services.GetRequiredService<IHouseholdService>();
        }
        public async Task<IEnumerable<Household>> GetMyHouseholds(int user_id)
        {
            _myHouseholds = await _householdService.GetMyHouseholdsAsync(user_id);

            return _myHouseholds;
        }
    }
}
