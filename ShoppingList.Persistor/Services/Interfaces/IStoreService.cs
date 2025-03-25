using ShoppingList.Core;

namespace ShoppingList.Persistor.Services.Interfaces
{
    internal interface IStoreService
    {
        Task<IEnumerable<Store>> GetStoresAsync(CancellationToken cancellationToken = default);
        Task<Store> GetStoreAsync(int store_id, CancellationToken cancellationToken = default);
        Task CreateStoreAsync(string name, CancellationToken cancellationToken = default);
        Task UpdateStoreAsync(int store_id, string name, CancellationToken cancellationToken = default);
        Task DeleteStoreAsync(int store_id, CancellationToken cancellationToken = default);
    }
}
