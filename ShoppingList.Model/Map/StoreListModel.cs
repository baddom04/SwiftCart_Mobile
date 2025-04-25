using Microsoft.Extensions.DependencyInjection;
using ShoppingList.Core;
using ShoppingList.Persistor;
using ShoppingList.Persistor.DTO;
using ShoppingList.Persistor.Services.Interfaces;

namespace ShoppingList.Model.Map
{
    public class StoreListModel
    {
        public int MaxPages { get; private set; }

        private IEnumerable<Store> _stores = [];
        private readonly IStoreService _storeService = AppServiceProvider.Services.GetRequiredService<IStoreService>();

        public async Task<IEnumerable<Store>> GetStoresAsync(string search, int page, LocationFilter locationFilter)
        {
            PaginatedResponse<Store> stores = await _storeService.GetStoresAsync(search, page, locationFilter);

            _stores = stores.QueryResult;

            MaxPages = stores.Meta is null ? stores.MaxPages : stores.Meta.MaxPages;

            return _stores;
        }
    }
}
