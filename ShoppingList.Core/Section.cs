namespace ShoppingList.Core
{
    public class Section
    {
        public int Id { get; init; }
        public required string Name { get; init; }
        public int MapId { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; init; }
    }
}
