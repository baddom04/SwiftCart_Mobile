using ShoppingList.Core;
using ShoppingList.Core.Enums;
using System.Collections.ObjectModel;

namespace ShoppingListEditor.Model.Editables
{
    public class MapSegmentEditable
    {
        public int Id { get; set; }    
        public int X { get; set; }
        public int Y { get; set; }
        public int MapId { get; set; }
        public int? SectionId { get; set; }

        private SegmentType _type;
        public SegmentType Type
        {
            get { return _type; }
            set { _type = value; TypeChanged?.Invoke(); }
        }
        public required ObservableCollection<ProductEditable> Products { get; set; }
        public event Action? TypeChanged;

        public static MapSegmentEditable FromMapSegment(MapSegment segment)
        {
            return new MapSegmentEditable()
            {
                Id = segment.Id,
                X = segment.X,
                Y = segment.Y,
                MapId = segment.MapId,
                SectionId = segment.SectionId,
                Type = segment.Type,
                Products = [.. segment.Products.Select(ProductEditable.FromProduct)],
            };
        }
    }
}
