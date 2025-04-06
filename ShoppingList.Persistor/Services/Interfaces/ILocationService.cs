using ShoppingList.Core;

namespace ShoppingList.Persistor.Services.Interfaces
{
    public interface ILocationService
    {
        Task<Location> CreateLocationAsync(int store_id, string country, string zip_code, string city, string street, string? detail, CancellationToken cancellationToken = default);
        Task<Location> GetLocationAsync(int store_id, CancellationToken cancellationToken = default);
        Task<Location> UpdateLocationAsync(int store_id, string country, string zip_code, string city, string street, string? detail, CancellationToken cancellationToken = default);
        Task DeleteLocationAsync(int store_id, CancellationToken cancellationToken = default);
    }
}
