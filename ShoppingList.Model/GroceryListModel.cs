//using ShoppingList.Core;
//using ShoppingList.Core.Enums;
//using ShoppingList.Model.Temp;
//using System.Collections.ObjectModel;

//namespace ShoppingList.Model
//{
//    public struct ItemEditor(ShoppingItem item, int index)
//    {
//        public ShoppingItem Item = item;
//        public int Index = index;
//    }
//    public class GroceryListModel
//    {
//        #region Validation

//        #region Properties
//        public bool IsValidItem { get; private set; }
//        public ItemFormErrorType? ErrorType { get; private set; }
//        #endregion

//        #region Events
//        public event EventHandler? ErrorTypeChanged;
//        #endregion

//        #region Methods
//        public void Validate(string itemName, string itemCount)
//        {
//            if (string.IsNullOrWhiteSpace(itemName))
//            {
//                ErrorType = ItemFormErrorType.EmptyName;
//                IsValidItem = false;
//            }
//            else
//            {
//                ErrorType = null;
//                IsValidItem = true;
//            }
//            ErrorTypeChanged?.Invoke(this, new EventArgs());
//        }
//        #endregion

//        #endregion

//        #region Item editing

//        #region Properties
//        public ItemEditor? EditedItem { get; private set; }
//        #endregion

//        #region Events
//        public event EventHandler? EditedItemChanged;
//        #endregion

//        #region Methods
//        public void StartEdit(ShoppingItem item, int index)
//        {
//            EditedItem = new(item, index);
//            EditedItemChanged?.Invoke(this, new EventArgs());
//        }
//        public void SaveEdit()
//        {
//            if (!EditedItem.HasValue) throw new InvalidOperationException("Can't save an item that is null!");

//            Validate(EditedItem.Value.Item.Name, EditedItem.Value.Item.Quantity);
//            if (!IsValidItem) return;

//            if (EditedItem.Value.Index < 0)
//            {
//                ShoppingList.Add(EditedItem.Value.Item);
//            }
//            else
//            {
//                ShoppingList[EditedItem.Value.Index] = EditedItem.Value.Item;
//            }
//            EditedItem = null;
//            EditedItemChanged?.Invoke(this, new EventArgs());
//        }
//        #endregion

//        #endregion

//        #region Properties
//        public ObservableCollection<ShoppingItem> ShoppingList { get; }
//        public IReadOnlyCollection<UnitType> UnitTypes { get; }
//        #endregion

//        #region Methods
//        public GroceryListModel()
//        {
//            //ShoppingList = [.. ShoppingListPersistor.LoadShoppingList()];
//            ShoppingList = [];
//            ShoppingList.CollectionChanged += (_, _) => Save();

//            UnitTypes = [.. (UnitType[])Enum.GetValues(typeof(UnitType))];
//        }
//        public void DeleteItem(ShoppingItem item)
//        {
//            ShoppingList.Remove(item);
//        }
//        public void AddItem(ShoppingItem item)
//        {
//            ShoppingList.Add(item);
//        }
//        public void AddComment(ShoppingItem item, User user, string comment)
//        {
//            item.Comments.Add(new Comment() { Content = comment, User = user });
//            Save();
//        }
//        private void Save()
//        {
//            //ShoppingListPersistor.SaveShoppingList(ShoppingList);
//        }
//        #endregion
//    }
//}
