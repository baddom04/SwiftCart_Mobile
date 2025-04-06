using ShoppingList.Core;

namespace ShoppingListEditor.Model.Editables
{
    public class ProductEditable
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Brand { get; set; }
        public string? Description { get; set; }
        public required uint Price { get; set; }
        public int MapSegmentId { get; set; }
        public static ProductEditable FromProduct(Product product)
        {
            return new ProductEditable()
            {
                Id = product.Id,
                Name = product.Name,
                Brand = product.Brand,
                Price = product.Price,
                MapSegmentId = product.MapSegmentId,
                Description = product.Description,
            };
        }
    }
}
