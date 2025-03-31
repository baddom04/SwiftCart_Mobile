using ShoppingList.Core;

namespace ShoppingList.Persistor.Services.Interfaces
{
    public interface IMapService
    {
        Task<Map> CreateMapAsync(int store_id, int x_size, int y_size, CancellationToken cancellationToken = default);
        Task<Map> GetMapAsync(int store_id, CancellationToken cancellationToken = default);
        Task<Map> UpdateMapAsync(int store_id, int x_size, int y_size, CancellationToken cancellationToken = default);
        Task DeleteMapAsync(int store_id, CancellationToken cancellationToken = default);
    }
}
