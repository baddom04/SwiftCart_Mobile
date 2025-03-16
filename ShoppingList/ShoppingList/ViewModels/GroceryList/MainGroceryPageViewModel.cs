using ShoppingList.ViewModels.Shared;

namespace ShoppingList.ViewModels.GroceryList
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
