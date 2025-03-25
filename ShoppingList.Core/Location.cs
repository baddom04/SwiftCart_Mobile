namespace ShoppingList.Core
{
    public class Location
    {
        public int Id { get; init; }
        public required string Country { get; init; }
        public required string ZipCode { get; init; }
        public required string City { get; init; }
        public required string Street { get; init; }
        public required string Detail { get; init; }
        public int StoreId { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; init; }
    }
}
