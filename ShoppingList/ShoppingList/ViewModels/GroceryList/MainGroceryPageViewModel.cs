using ReactiveUI;

namespace ShoppingList.ViewModels.GroceryList
{
    internal class MainGroceryPageViewModel : ViewModelBase
    {
        private ViewModelBase _currentPage;
        public ViewModelBase CurrentPage
        {
            get { return _currentPage; }
            private set { this.RaiseAndSetIfChanged(ref _currentPage, value); }
        }
    }
}
