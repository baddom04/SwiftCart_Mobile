namespace ShoppingList.Core
{
    public class Product
    {
        public int Id { get; init; }
        public required string Name { get; init; }
        public required string Brand { get; init; }
        public required string Description { get; init; }
        public int MapSegmentId { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; init; }
    }
}
