using ShoppingList.Core;

namespace ShoppingList.Model.Map
{
    public class MapModel(Store store)
    {
        public Store Store { get; } = store;
        public List<Product> SelectedProducts { get; } = [];

        private readonly HashSet<int> _markedMapSegmentIds = [];
        private Section? _selectedSection;

        public void Select(Product product)
        {
            SelectedProducts.Add(product);
            _markedMapSegmentIds.Add(product.MapSegmentId);
        }
        public void UnSelect(Product product)
        {
            SelectedProducts.Remove(product);
            _markedMapSegmentIds.Remove(product.MapSegmentId);
        }
        public void SelectSection(Section section)
        {
            if (section.MapId != Store.Map!.Id)
                throw new ArgumentException("The provided section do not belong to this store.");

            _selectedSection = section;

            Store.Map!.MapSegments
                    .Where(ms => section.Id == -1 ? ms.SectionId == null : ms.SectionId == section.Id)
                    .Select(ms => ms.Id)
                    .ToList()
                    .ForEach(id => _markedMapSegmentIds.Add(id));
        }
        public void UnSelectSection()
        {
            if (_selectedSection == null)
                throw new InvalidOperationException("There is no selected section");
            if (_selectedSection.MapId != Store.Map!.Id)
                throw new ArgumentException("The provided section do not belong to this store.");


            Store.Map!.MapSegments
                    .Where(ms => _selectedSection.Id == -1 ? ms.SectionId == null : ms.SectionId == _selectedSection.Id)
                    .Select(ms => ms.Id)
                    .ToList()
                    .ForEach(id => _markedMapSegmentIds.Remove(id));

            _selectedSection = null;
        }
        public void Clear()
        {
            _markedMapSegmentIds.Clear();
            Store.Map!.MapSegments.ToList().ForEach(ms => ms.Marked = false);
        }
        public void MarkMapSegments()
        {
            Store.Map!.MapSegments
                .Where(ms => ms.Type == Core.Enums.SegmentType.Shelf || ms.Type == Core.Enums.SegmentType.Fridge)
                .ToList()
                .ForEach(ms => 
                {
                    if (_markedMapSegmentIds.Contains(ms.Id)) 
                        ms.Marked = true; 
                    else 
                        ms.Marked = false; 
                });
        }
    }
}
