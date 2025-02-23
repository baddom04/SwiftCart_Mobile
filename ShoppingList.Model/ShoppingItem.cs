using System.Collections.ObjectModel;

namespace ShoppingList.Model
{
    public class ShoppingItem(string name, int userId, ObservableCollection<Comment> comments, string quantity = "", UnitType unit = UnitType.pieces, string? description = null) : ICloneable
    {
        public int UserId { get; set; } = userId;
        public string Name { get; set; } = name;
        public string Quantity { get; set; } = quantity;
        public UnitType Unit { get; set; } = unit;
        public string? Description { get; set; } = description;
        public ObservableCollection<Comment> Comments { get; set; } = comments;
        public static ShoppingItem Empty => new(string.Empty, -1, []);
        public object Clone()
        {
            return new ShoppingItem(Name, UserId, [.. Comments?.Select(com => new Comment() { Content = com.Content})], Quantity, Unit, Description);
        }
    }
}
