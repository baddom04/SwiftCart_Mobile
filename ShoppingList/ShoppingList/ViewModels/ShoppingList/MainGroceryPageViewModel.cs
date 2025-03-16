using ShoppingList.Model.Settings;
using ShoppingList.Model.Social;
using ShoppingList.Utils;
using ShoppingList.ViewModels.Shared;
using System;

namespace ShoppingList.ViewModels.ShoppingList
{
    internal class MainGroceryPageViewModel : MainViewModelBase<GroceryPage>
    {
        public MainGroceryPageViewModel(UserAccountModel account, MyHouseholdsModel myHouseholds, Action<NotificationType, string> showNotification)
        {
            _pages = new()
            {
                { GroceryPage.Main, new HouseholdsGroceriesViewModel(account, myHouseholds, ChangeToPage, showNotification, ChangePage) }
            };
            _currentPage = _pages[GroceryPage.Main];
        }

        public override void ChangeToDefaultPage()
        {
            CurrentPage = _pages[GroceryPage.Main];
        }

        public void ChangeToPage(ViewModelBase page)
        {
            CurrentPage = page;
        }
    }
    internal enum GroceryPage
    {
        Main
    }
}
