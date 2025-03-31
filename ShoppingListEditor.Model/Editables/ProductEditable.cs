using ShoppingList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShoppingListEditor.Model.Editables
{
    public class ProductEditable
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Brand { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
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
