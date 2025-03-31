using ShoppingList.Core;

namespace ShoppingListEditor.Model.Editables
{
    public class MapEditable
    {
        public int Id { get; set; }
        public int XSize { get; set; }
        public int YSize { get; set; }
        public int StoreId { get; set; }
        public required IEnumerable<MapSegmentEditable> MapSegments { get; set; }
        public required IEnumerable<SectionEditable> Sections { get; set; }

        public static MapEditable? FromMap(Map? map)
        {
            if (map is null) return null;

            return new MapEditable()
            {
                Id = map.Id,
                XSize = map.XSize,
                YSize = map.YSize,
                StoreId = map.StoreId,
                MapSegments = [.. map.MapSegments.Select(MapSegmentEditable.FromMapSegment)],
                Sections = [.. map.Sections.Select(SectionEditable.FromSection)],
            };
        }
    }
}
