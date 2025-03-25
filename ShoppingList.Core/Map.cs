namespace ShoppingList.Core
{
    public class Map
    {
        public int Id { get; init; }
        public int XSize { get; init; }
        public int YSize { get; init; }
        public int StoreId { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; init; }
        public required IEnumerable<Section> Sections { get; init; }
        public required IEnumerable<MapSegment> MapSegments { get; init; }
    }
}
