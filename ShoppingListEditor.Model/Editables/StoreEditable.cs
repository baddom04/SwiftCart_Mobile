using ShoppingList.Core;

namespace ShoppingListEditor.Model.Editables
{
    public class StoreEditable
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required bool Published { get; set; }
        public LocationEditable? Location { get; set; }
        public MapEditable? Map { get; set; }

        public static StoreEditable? FromStore(Store? store)
        {
            if (store == null) return null;

            return new StoreEditable() 
            { 
                Id = store.Id, 
                Name = store.Name,
                Location = LocationEditable.FromLocation(store.Location),
                Map = MapEditable.FromMap(store.Map),
                Published = store.Published,
            };
        }
    }
}
