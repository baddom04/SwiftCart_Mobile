﻿using ShoppingList.Core;
using ShoppingList.Model.Settings;
using ShoppingList.Model.ShoppingList;
using ShoppingList.Model.Social;
using ShoppingList.Utils;
using ShoppingList.ViewModels.Shared;
using System;

namespace ShoppingList.ViewModels.ShoppingList
{
    internal class MainGroceryPageViewModel : MainViewModelBase<GroceryPage>
    {
        private readonly CreateGroceryViewModel _groceryEditingPage;
        public MainGroceryPageViewModel(UserAccountModel account, MyHouseholdsModel myHouseholds, Action<NotificationType, string> showNotification, Action<bool> showLoading)
        {
            _groceryEditingPage = new(new CreateGroceryModel(), showLoading);

            _pages = new()
            {
                { GroceryPage.Main, new HouseholdsGroceriesViewModel(account, myHouseholds, ChangeToPage, showNotification, ChangePage, ChangeToEditingPage) },
            };
            _currentPage = _pages[GroceryPage.Main];
        }

        public override void ChangeToDefaultPage()
        {
            CurrentPage = _pages[GroceryPage.Main];
        }

        private void ChangeToPage(ViewModelBase page)
        {
            CurrentPage = page;
        }

        private void ChangeToEditingPage(int householdId, Grocery? grocery, Action goBackAction)
        {
            _groceryEditingPage.Initialize(householdId, grocery, goBackAction);
            CurrentPage = _groceryEditingPage;
        }
    }
    internal enum GroceryPage
    {
        Main,
    }
}
