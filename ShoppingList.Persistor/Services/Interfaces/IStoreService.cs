using ShoppingList.Core;
using ShoppingList.Persistor.DTO;

namespace ShoppingList.Persistor.Services.Interfaces
{
    public interface IStoreService
    {
        Task<Store?> GetMyStoreAsync(CancellationToken cancellationToken = default);
        Task<PaginatedResponse<Store>> GetStoresAsync(string search, int page, CancellationToken cancellationToken = default);
        Task<Store> GetStoreAsync(int store_id, CancellationToken cancellationToken = default);
        Task<Store> CreateStoreAsync(string name, CancellationToken cancellationToken = default);
        Task<Store> UpdateStoreAsync(int store_id, string name, CancellationToken cancellationToken = default);
        Task DeleteStoreAsync(int store_id, CancellationToken cancellationToken = default);
    }
}
