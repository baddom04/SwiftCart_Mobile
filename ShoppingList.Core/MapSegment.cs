using ShoppingList.Core.Enums;

namespace ShoppingList.Core
{
    public class MapSegment
    {
        public int Id { get; init; }
        public int X { get; init; }
        public int Y { get; init; }
        public int MapId { get; init; }
        public int? SectionId { get; init; }
        public SegmentType Type { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; init; }
        public required IEnumerable<Product> Products { get; init; }
    }
}
