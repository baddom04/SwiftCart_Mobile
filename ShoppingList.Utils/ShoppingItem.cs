using System.Collections.ObjectModel;

namespace ShoppingList.Utils
{
    public class ShoppingItem(string name, User owner, ObservableCollection<Comment> comments, string quantity = "", UnitType unit = UnitType.Pieces, string? description = null) : ICloneable
    {
        public User Owner { get; set; } = owner;
        public string Name { get; set; } = name;
        public string Quantity { get; set; } = quantity;
        public UnitType Unit { get; set; } = unit;
        public string? Description { get; set; } = description;
        public ObservableCollection<Comment> Comments { get; set; } = comments;
        public static ShoppingItem Empty => new(string.Empty, User.Empty, []);
        public object Clone()
        {
            return new ShoppingItem(Name, (Owner.Clone() as User)!, [.. Comments?.Select(com => new Comment((com.User.Clone() as User)!, com.Text))], Quantity, Unit, Description);
        }
    }
}
