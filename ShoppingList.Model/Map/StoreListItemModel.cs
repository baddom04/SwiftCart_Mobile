using Microsoft.Extensions.DependencyInjection;
using ShoppingList.Core;
using ShoppingList.Persistor;
using ShoppingList.Persistor.Services.Interfaces;

namespace ShoppingList.Model.Map
{
    public class StoreListItemModel(Store storeWithoutMap)
    {
        public Store StoreWithoutMap { get; } = storeWithoutMap;

        private readonly IStoreService _storeService = AppServiceProvider.Services.GetRequiredService<IStoreService>();
        private readonly int _storeId = storeWithoutMap.Id;
        public async Task<Store> GetFullStoreAsync()
        {
            Store storeWithMap = await _storeService.GetStoreAsync(_storeId);

            return storeWithMap;
        }
    }
}
