using Microsoft.Extensions.DependencyInjection;
using ShoppingList.Core;
using ShoppingList.Persistor;
using ShoppingList.Persistor.Services.Interfaces;
using ShoppingListEditor.Model.Editables;

namespace ShoppingListEditor.Model
{
    public class EditorModel
    {
        private StoreEditable? _store;
        public StoreEditable? Store
        {
            get { return _store; }
            private set { _store = value; StoreChanged?.Invoke(); }
        }
        public event Action? StoreChanged;

        private readonly IStoreService _storeService = AppServiceProvider.Services.GetRequiredService<IStoreService>();
        private readonly ILocationService _locationService = AppServiceProvider.Services.GetRequiredService<ILocationService>();
        private readonly IMapService _mapService = AppServiceProvider.Services.GetRequiredService<IMapService>();

        public async Task<StoreEditable?> GetUsersStoreAsync()
        {
            Store = StoreEditable.FromStore(await _storeService.GetMyStoreAsync());
            return Store;
        }
        public async Task CreateStoreAsync(string name)
        {
            if (Store is not null)
                throw new InvalidOperationException("Store already exists");

            Store newStore = await _storeService.CreateStoreAsync(name);
            Store = new StoreEditable() { Name = newStore.Name, Id = newStore.Id };
        }
        public async Task UpdateStoreAsync(string name)
        {
            if (Store is null)
                throw new InvalidOperationException("Store does not exist");

            Store newStore = await _storeService.UpdateStoreAsync(Store.Id, name);
            Store.Name = newStore.Name;
        }
        public async Task DeleteStoreAsync()
        {
            if (Store is null || Store.Id == default)
                throw new InvalidOperationException("Store does not exist");

            await _storeService.DeleteStoreAsync(Store.Id);
            Store = null;
        }
        public async Task CreateLocationAsync(string country, string zip_code, string city, string street, string detail)
        {
            if (Store is null || Store.Id == default)
                throw new InvalidOperationException("The store to add the location to, does not exist");
            if (Store.Location is not null)
                throw new InvalidOperationException("Location already exists");

            Location location = await _locationService.CreateLocationAsync(Store.Id, country, zip_code, city, street, detail);
            Store.Location = new LocationEditable()
            {
                Id = location.Id,
                Country = location.Country,
                Street = location.Street,
                Detail = location.Detail,
                City = location.City,
                ZipCode = location.ZipCode,
                StoreId = location.StoreId,
            };
        }
        public async Task UpdateLocationAsync(string country, string zip_code, string city, string street, string detail)
        {
            if (Store is null || Store.Id == default)
                throw new InvalidOperationException("The store to add the location to does not exist.");
            if (Store.Location is null)
                throw new InvalidOperationException("Location does not exist");

            Location location = await _locationService.UpdateLocationAsync(Store.Id, country, zip_code, city, street, detail);
            Store.Location.Country = location.Country;
            Store.Location.ZipCode = location.ZipCode;
            Store.Location.Detail = location.Detail;
            Store.Location.City = location.City;
            Store.Location.Street = location.Street;
        }
        public async Task CreateMapAsync(int x_size, int y_size)
        {
            if (Store is null || Store.Id == default)
                throw new InvalidOperationException("The store to add the location to does not exist.");
            if (Store.Map is not null)
                throw new InvalidOperationException("Map already exists");

            Map map = await _mapService.CreateMapAsync(Store.Id, x_size, y_size);
            Store.Map = new MapEditable()
            {
                Id = map.Id,
                XSize = map.XSize,
                YSize = map.YSize,
                StoreId = map.StoreId,
                MapSegments = [],
                Sections = [],
            };
        }
        public async Task UpdateMapAsync(int x_size, int y_size)
        {
            if (Store is null || Store.Id == default)
                throw new InvalidOperationException("The store to add the location to does not exist.");
            if (Store.Map is null)
                throw new InvalidOperationException("Map does not exist");

            Map map = await _mapService.UpdateMapAsync(Store.Id, x_size, y_size);
            Store.Map.XSize = map.XSize;
            Store.Map.YSize = map.YSize;
        }
    }
}
