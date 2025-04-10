using ShoppingList.Core;

namespace ShoppingList.Persistor.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync(int map_id, CancellationToken cancellationToken = default);
        Task<Product> GetProductAsync(int product_id, CancellationToken cancellationToken = default);
        Task<Product> CreateProductAsync(int segment_id, string name, string brand, string? description, uint price, CancellationToken cancellationToken = default);
        Task<Product> UpdateProductAsync(int segment_id, int product_id, string name, string brand, string? description, uint price, CancellationToken cancellationToken = default);
        Task DeleteProductAsync(int segment_id, int product_id, CancellationToken cancellationToken = default);
        Task UpdateProductSegmentAsync(int old_segment_id, int product_id, int segment_id, CancellationToken cancellationToken = default);
    }
}
