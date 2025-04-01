using ShoppingList.Core;
using ShoppingList.Core.Enums;

namespace ShoppingListEditor.Model.Editables
{
    public class MapEditable
    {
        public int Id { get; set; }
        public int XSize { get; private set; }
        public int YSize { get; private set; }
        public int StoreId { get; set; }
        public required MapSegmentEditable[,] MapSegments { get; set; }
        public required List<SectionEditable> Sections { get; set; }

        public static MapEditable? FromMap(Map? map)
        {
            if (map is null) return null;

            IEnumerable<MapSegmentEditable> segments = map.MapSegments.Select(MapSegmentEditable.FromMapSegment);
            MapSegmentEditable[,] segmentMatrix = new MapSegmentEditable[map.YSize, map.XSize];

            foreach (MapSegmentEditable segment in segments)
            {
                segmentMatrix[segment.Y, segment.X] = segment;
            }

            for (int y = 0; y < map.YSize; y++)
            {
                for (int x = 0; x < map.XSize; x++)
                {
                    if (segmentMatrix[y, x] == default)
                    {
                        segmentMatrix[y, x] = new MapSegmentEditable()
                        {
                            X = x,
                            Y = y,
                            MapId = map.Id,
                            Products = [],
                            Type = SegmentType.Empty
                        };
                    }
                }
            }

            return new MapEditable()
            {
                Id = map.Id,
                XSize = map.XSize,
                YSize = map.YSize,
                StoreId = map.StoreId,
                MapSegments = segmentMatrix,
                Sections = [.. map.Sections.Select(SectionEditable.FromSection)],
            };
        }
        public void SetSizes(int x_size, int y_size)
        {
            MapSegmentEditable[,] newSegments = new MapSegmentEditable[y_size, x_size];

            for (int y = 0; y < y_size; y++)
            {
                for (int x = 0; x < x_size; x++)
                {
                    if (y < MapSegments.GetLength(0) && x < MapSegments.GetLength(1))
                    {
                        newSegments[y, x] = MapSegments[y, x];
                    }
                    else
                    {
                        newSegments[y, x] = new MapSegmentEditable()
                        {
                            X = x,
                            Y = y,
                            MapId = Id,
                            Products = [],
                            Type = SegmentType.Empty
                        };
                    }
                }
            }

            MapSegments = newSegments;
        }
    }
}
