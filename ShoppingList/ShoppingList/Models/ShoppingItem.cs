using System;

namespace ShoppingList.Models
{
    internal class ShoppingItem(string name, User owner, string quantity = "", UnitType unit = UnitType.Pieces, string? description = null) : ICloneable
    {
        public User Owner { get; set; } = owner;
        public string Name { get; set; } = name;
        public string Quantity { get; set; } = quantity;
        public UnitType Unit { get; set; } = unit;
        public string? Description { get; set; } = description;
        public static ShoppingItem Empty => new(string.Empty, App.CurrentUser);

        public object Clone()
        {
            return new ShoppingItem(Name, new User(Owner.UserName, Owner.NickName, Owner.Email), Quantity, Unit, Description);
        }
    }
}
