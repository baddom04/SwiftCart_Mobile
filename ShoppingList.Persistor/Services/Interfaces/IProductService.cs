using ShoppingList.Core;

namespace ShoppingList.Persistor.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync(int map_id, CancellationToken cancellationToken = default);
        Task<Product> GetProductAsync(int product_id, CancellationToken cancellationToken = default);
        Task<Product> CreateProductAsync(int segment_id, string name, string brand, string description, decimal price, CancellationToken cancellationToken = default);
        Task<Product> UpdateProductAsync(int segment_id, int product_id, string name, string brand, string description, decimal price, CancellationToken cancellationToken = default);
        Task DeleteProductAsync(int segment_id, int product_id, CancellationToken cancellationToken = default);
    }
}
