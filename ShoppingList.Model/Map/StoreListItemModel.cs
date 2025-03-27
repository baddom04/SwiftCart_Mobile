using Microsoft.Extensions.DependencyInjection;
using ShoppingList.Core;
using ShoppingList.Persistor;
using ShoppingList.Persistor.Services.Interfaces;

namespace ShoppingList.Model.Map
{
    public class StoreListItemModel(Store storeWithoutMap)
    {
        private readonly IStoreService _storeService = AppServiceProvider.Services.GetRequiredService<IStoreService>();

        private readonly int _storeId = storeWithoutMap.Id;

        public Store StoreWithoutMap { get; } = storeWithoutMap;

        public async Task<Store> GetFullStoreAsync()
        {
            Store storeWithMap = await _storeService.GetStoreAsync(_storeId);

            return storeWithMap;
        }
    }
}
