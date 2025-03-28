using ShoppingList.Shared.Model.Settings;
using ShoppingList.Shared.ViewModels;

namespace ShoppingList.ViewModels
{
    public class MobileMainViewModel : MainViewModel
    {
        public MobileMainViewModel(UserAccountModel account) : base(account) 
        {
            _pages[MainPage.Main] = new LoggedInViewModel(account, ChangePage, ShowLoading, ShowNotificationDialog);
        }
    }
}
