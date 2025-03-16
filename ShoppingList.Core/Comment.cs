namespace ShoppingList.Core
{
    public class Comment
    {
        public int Id { get; init; }
        public int UserId { get; init; }
        public int GroceryId { get; init; }
        public required string Content { get; init; }

        public required User User { get; init; }
    }
}
