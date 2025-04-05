using Microsoft.Extensions.DependencyInjection;
using ShoppingList.Core;
using ShoppingList.Core.Enums;
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
        public event Action? MapChanged;
        public event Action? LocationChanged;
        public event Action? SectionsChanged;

        private readonly IStoreService _storeService = AppServiceProvider.Services.GetRequiredService<IStoreService>();
        private readonly ILocationService _locationService = AppServiceProvider.Services.GetRequiredService<ILocationService>();
        private readonly IMapService _mapService = AppServiceProvider.Services.GetRequiredService<IMapService>();
        private readonly IMapSegmentService _segmentService = AppServiceProvider.Services.GetRequiredService<IMapSegmentService>();
        private readonly ISectionService _sectionService = AppServiceProvider.Services.GetRequiredService<ISectionService>();
        private readonly IProductService _productService = AppServiceProvider.Services.GetRequiredService<IProductService>();

        public EditorModel()
        {
            StoreChanged += EditorModel_StoreChanged;
        }

        private void EditorModel_StoreChanged()
        {
            MapChanged?.Invoke();
            SectionsChanged?.Invoke();
            LocationChanged?.Invoke();
        }
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
            Store.Location = LocationEditable.FromLocation(location);
            LocationChanged?.Invoke();
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
            LocationChanged?.Invoke();
        }
        public async Task CreateMapAsync(int x_size, int y_size)
        {
            if (Store is null || Store.Id == default)
                throw new InvalidOperationException("The store to add the location to does not exist.");
            if (Store.Map is not null)
                throw new InvalidOperationException("Map already exists");

            Map map = await _mapService.CreateMapAsync(Store.Id, x_size, y_size);
            Store.Map = MapEditable.FromMap(map);
            MapChanged?.Invoke();
        }
        public async Task UpdateMapAsync(int x_size, int y_size)
        {
            if (Store is null || Store.Id == default)
                throw new InvalidOperationException("The store to add the location to does not exist");
            if (Store.Map is null)
                throw new InvalidOperationException("Map does not exist");

            Map map = await _mapService.UpdateMapAsync(Store.Id, x_size, y_size);
            Store.Map.SetSizes(map.XSize, map.YSize);
            MapChanged?.Invoke();
        }
        public async Task UploadSegmentAsync(MapSegmentEditable segment)
        {
            if (Store is null || Store.Id == default)
                throw new InvalidOperationException("The store to add the location to does not exist");
            if (Store.Map is null)
                throw new InvalidOperationException("Map does not exist");

            if (Store.Map.MapSegments[segment.Y, segment.X] != segment)
                throw new InvalidDataException("The given segment is not the same as in the Store model");

            MapSegment createdSegment = segment.Id == default 
                ? await _segmentService.CreateMapSegmentAsync(segment.MapId, segment.X, segment.Y, segment.Type, segment.SectionId)
                : await _segmentService.UpdateMapSegmentAsync(segment.MapId, segment.Id, segment.X, segment.Y, segment.Type, segment.SectionId);

            segment.Id = createdSegment.Id;
            segment.X = createdSegment.X;
            segment.Y = createdSegment.Y;
            segment.Type = createdSegment.Type;
            segment.SectionId = createdSegment.SectionId;
        }
        public async Task DeleteSegmentAsync(MapSegmentEditable segment)
        {
            if (Store is null || Store.Id == default)
                throw new InvalidOperationException("The store to add the location to does not exist");
            if (Store.Map is null)
                throw new InvalidOperationException("Map does not exist");

            if (Store.Map.MapSegments[segment.Y, segment.X] != segment)
                throw new InvalidDataException("The given segment is not the same as in the Store model");
            if(segment.Id == default)
                throw new InvalidDataException("The given segment does not have an Id");

            await _segmentService.DeleteMapSegmentAsync(segment.MapId, segment.Id);

            segment.Id = default;
            segment.SectionId = null;
            segment.Products.Clear();
            segment.Type = SegmentType.Empty;
        }
        public async Task ChangeSectionOnSegmentAsync(MapSegmentEditable segment, SectionEditable section)
        {
            if (Store is null || Store.Id == default)
                throw new InvalidOperationException("The store to add the location to does not exist");
            if (Store.Map is null)
                throw new InvalidOperationException("Map does not exist");

            if (Store.Map.MapSegments[segment.Y, segment.X] != segment)
                throw new InvalidDataException("The given segment is not the same as in the Store model");
            if (!Store.Map.Sections.Contains(section) && section.Id != -1)
                throw new InvalidDataException("Section does not exist in the collection");
            if (segment.Id == default)
                throw new InvalidDataException("The given segment does not have an Id");
            if (section.Id == default)
                throw new InvalidDataException("The given section does not have an Id");

            MapSegment updated = await _segmentService.UpdateMapSegmentAsync(segment.MapId, segment.Id, segment.X, segment.Y, segment.Type, section.Id == -1 ? null : section.Id);

            segment.Id = updated.Id;
            segment.X = updated.X;
            segment.Y = updated.Y;
            segment.Type = updated.Type;
            segment.SectionId = updated.SectionId;
        }
        public async Task AddSectionAsync(string name)
        {
            if (Store is null || Store.Id == default)
                throw new InvalidOperationException("The store to add the location to does not exist");
            if (Store.Map is null)
                throw new InvalidOperationException("Map does not exist");
            if (Store.Map.Sections.Any(s => s.Name == name))
                throw new InvalidOperationException("This section already exists");

            Section? section = await _sectionService.CreateSectionAsync(Store.Map.Id, name);

            Store.Map.Sections.Add(SectionEditable.FromSection(section));

            SectionsChanged?.Invoke();
        }
        public async Task RemoveSectionAsync(SectionEditable section)
        {
            if (Store is null || Store.Id == default)
                throw new InvalidOperationException("The store to add the location to does not exist");
            if (Store.Map is null)
                throw new InvalidOperationException("Map does not exist");
            if (!Store.Map.Sections.Contains(section))
                throw new InvalidDataException("Section is not part of the collection");
            if (section.Id == default)
                throw new InvalidDataException("The given section does not have an Id");

            await _sectionService.DeleteSectionAsync(Store.Map.Id, section.Id);

            Store.Map.Sections.Remove(section);

            SectionsChanged?.Invoke();
        }
        public async Task UpdateSectionAsync(SectionEditable section)
        {
            if (Store is null || Store.Id == default)
                throw new InvalidOperationException("The store to add the location to does not exist");
            if (Store.Map is null)
                throw new InvalidOperationException("Map does not exist");
            if (!Store.Map.Sections.Contains(section))
                throw new InvalidDataException("Section is not part of the collection");
            if (section.Id == default)
                throw new InvalidDataException("The given section does not have an Id");

            await _sectionService.UpdateSectionAsync(Store.Map.Id, section.Id, section.Name);

            SectionsChanged?.Invoke();
        }
        public async Task DeleteProductAsync(MapSegmentEditable segment, ProductEditable product)
        {
            if (Store is null || Store.Id == default)
                throw new InvalidOperationException("The store to add the location to does not exist");
            if (Store.Map is null)
                throw new InvalidOperationException("Map does not exist");
            if (!segment.Products.Contains(product))
                throw new InvalidDataException("The given product is not part of the collection");
            if (segment.Id != product.MapSegmentId)
                throw new InvalidDataException("The given product does not belong to the given segment");

            await _productService.DeleteProductAsync(segment.Id, product.Id);

            segment.Products.Remove(product);
        }
        public async Task CreateProductAsync(MapSegmentEditable segment, string nameInput, string descriptionInput, string brandInput, string priceInput)
        {
            if (Store is null || Store.Id == default)
                throw new InvalidOperationException("The store to add the location to does not exist");
            if (Store.Map is null)
                throw new InvalidOperationException("Map does not exist");
            if (!decimal.TryParse(priceInput, out var price))
                throw new InvalidDataException("Price is not in the correct format");

            Product product = await _productService.CreateProductAsync(segment.Id, nameInput, descriptionInput, brandInput, price);

            segment.Products.Add(ProductEditable.FromProduct(product));
        }
        public async Task UpdateProductAsync(MapSegmentEditable segment, int productId, string nameInput, string descriptionInput, string brandInput, string priceInput)
        {
            if (Store is null || Store.Id == default)
                throw new InvalidOperationException("The store to add the location to does not exist");
            if (Store.Map is null)
                throw new InvalidOperationException("Map does not exist");
            if (!decimal.TryParse(priceInput, out var price))
                throw new InvalidDataException("Price is not in the correct format");

            Product product = await _productService.UpdateProductAsync(segment.Id, productId, nameInput, descriptionInput, brandInput, price);

            int index = segment.Products.IndexOf(segment.Products.First(p => p.Id == productId));
            segment.Products.RemoveAt(index);
            segment.Products.Insert(index, ProductEditable.FromProduct(product));
        }
    }
}
