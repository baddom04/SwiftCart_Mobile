namespace ShoppingList.Core
{
    public class Grocery
    {
        public int Id { get; init; }
        public required string Name { get; init; }
        public int? Quantity { get; init; }
        public UnitType? Unit { get; init; }
        public string? Description { get; init; }
        public int HouseholdId { get; init; }
        public int UserId { get; init; }
    }
}
