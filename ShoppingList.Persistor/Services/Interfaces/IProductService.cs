using ShoppingList.Core;

namespace ShoppingList.Persistor.Services.Interfaces
{
    internal interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync(int map_id, CancellationToken cancellationToken = default);
        Task<Product> GetProductAsync(int product_id, CancellationToken cancellationToken = default);
        Task CreateProductAsync(int segment_id, string name, string brand, string description, CancellationToken cancellationToken = default);
        Task UpdateProductAsync(int segment_id, int product_id, string name, string brand, string description, CancellationToken cancellationToken = default);
        Task DeleteProductAsync(int segment_id, int product_id, CancellationToken cancellationToken = default);
    }
}
