namespace ShoppingList.Core
{
    public class Store
    {
        public int Id { get; init; }
        public required string Name { get; init; }
        public int UserId { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; init; }
        public required Location Location { get; init; }
        public Map? Map { get; init; }
    }
}
