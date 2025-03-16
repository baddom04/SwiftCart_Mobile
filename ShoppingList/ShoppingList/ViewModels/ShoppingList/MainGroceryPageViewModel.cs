using ShoppingList.ViewModels.Shared;

namespace ShoppingList.ViewModels.ShoppingList
{
    internal class MainGroceryPageViewModel : MainViewModelBase<GroceryPage>
    {
        public MainGroceryPageViewModel()
        {
            _currentPage = new GroceryListViewModel();
        }

        public override void ChangeToDefaultPage()
        {
            // TODO: implement
        }
    }
    internal enum GroceryPage
    {
        Main
    }
}
