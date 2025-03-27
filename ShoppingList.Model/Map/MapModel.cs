using ShoppingList.Core;

namespace ShoppingList.Model.Map
{
    public class MapModel
    {
        public Store Store { get; }
        public MapModel(Store store)
        {
            Store = store;
        }
    }
}
