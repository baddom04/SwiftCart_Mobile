using Microsoft.Extensions.DependencyInjection;
using ShoppingList.Core;
using ShoppingList.Persistor;
using ShoppingList.Persistor.Services.Interfaces;

namespace ShoppingList.Model.Models
{
    public class MainSocialPanelModel
    {
        private readonly IHouseholdService _householdService;
        IEnumerable<Household>? _loadedHouseholds;
        public MainSocialPanelModel()
        {
            _householdService = AppServiceProvider.Services.GetRequiredService<IHouseholdService>();
        }

        public async Task<IEnumerable<Household>> GetHouseholdsAsync(string search, int page)
        {
            _loadedHouseholds = await _householdService.GetAllHouseholdsAsync(search, page);

            return _loadedHouseholds;
        }
    }
}
