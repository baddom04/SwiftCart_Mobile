namespace ShoppingList.Models
{
    public class ShoppingItem(string name, int quantity = 1, bool bought = false)
    {
        public string Name { get; set; } = name;
        public int Quantity { get; set; } = quantity;
        public bool Bought { get; set; } = bought;
    }
}
