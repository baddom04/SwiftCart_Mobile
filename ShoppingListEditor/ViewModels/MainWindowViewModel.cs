using ShoppingList.Shared.Model.Settings;
using ShoppingList.Shared.ViewModels;

namespace ShoppingListEditor.ViewModels
{
    public class MainWindowViewModel : MainViewModel
    {
        public MainWindowViewModel(UserAccountModel account) : base(account)
        {
            _pages[MainPage.Main] = new LoggedInViewModel();
        }
    }
}
