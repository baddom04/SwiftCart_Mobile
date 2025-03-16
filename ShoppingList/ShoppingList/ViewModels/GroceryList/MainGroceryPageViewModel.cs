using ReactiveUI;
using ShoppingList.ViewModels.Shared;

namespace ShoppingList.ViewModels.GroceryList
{
    internal class MainGroceryPageViewModel : DefaultPageOnChangeViewModel
    {
        private ViewModelBase _currentPage;
        public ViewModelBase CurrentPage
        {
            get { return _currentPage; }
            private set { this.RaiseAndSetIfChanged(ref _currentPage, value); }
        }

        public MainGroceryPageViewModel()
        {
            _currentPage = new GroceryListViewModel();
        }

        public override void ChangeToDefaultPage()
        {
            // TODO: implement
        }
    }
}
