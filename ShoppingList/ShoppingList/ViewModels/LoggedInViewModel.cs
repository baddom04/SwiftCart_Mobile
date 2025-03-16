﻿using ReactiveUI;
using ShoppingList.Model.Settings;
using ShoppingList.Utils;
using ShoppingList.ViewModels.GroceryList;
using ShoppingList.ViewModels.Map;
using ShoppingList.ViewModels.Settings;
using ShoppingList.ViewModels.Shared;
using ShoppingList.ViewModels.Social;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels
{
    internal class LoggedInViewModel : ViewModelBase
    {
        private DefaultPageOnChangeViewModel _currentPage;
        public DefaultPageOnChangeViewModel CurrentPage
        {
            get { return _currentPage; }
            set { this.RaiseAndSetIfChanged(ref _currentPage, value); }
        }

        private MenuIcon _selectedMenuItem;
        public MenuIcon SelectedMenuItem
        {
            get { return _selectedMenuItem; }
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedMenuItem, value);
                CurrentPage.ChangeToDefaultPage();
                CurrentPage = Menus[value];
            }
        }
        public ObservableCollection<MenuIcon> MenuItems { get; }

        public Dictionary<MenuIcon, DefaultPageOnChangeViewModel> Menus { get; }

        private readonly UserAccountModel _userAccount;
        private readonly Action<NotificationType, string> _showNotification;
        private readonly Action<bool> _showLoading;
        public LoggedInViewModel(UserAccountModel userAccount, Action<MainPage> changePage, Action<bool> showLoading, Action<NotificationType, string> showNotification)
        {

            Menus = new Dictionary<MenuIcon, DefaultPageOnChangeViewModel>
            {
                { new MenuIcon("Map", "globe_regular"), new MapViewModel() },
                { new MenuIcon("Shopping list", "cart_regular"), new GroceryListViewModel() },
                { new MenuIcon("Social", "people_regular"), new MainSocialPanelViewModel(userAccount, showNotification, showLoading) },
                { new MenuIcon("Settings", "settings_regular"), new MainSettingsViewModel(userAccount, showLoading, showNotification, changePage) },
            };

            MenuItems = [.. Menus.Select(item => item.Key)];
            _selectedMenuItem = MenuItems[2];
            _currentPage = Menus[_selectedMenuItem];
            _userAccount = userAccount;
            _showLoading = showLoading;
            _showNotification = showNotification;
        }

        public async Task GetUser()
        {
            _showLoading(true);

            try
            {
                await _userAccount.GetUserAsync();
            }
            catch (Exception ex)
            {
                string message = $"{StringProvider.GetString("DeleteUserError")}{ex.Message}";
                _showNotification(NotificationType.Error, message);
            }
            finally
            {
                _showLoading(false);
            }
        }
    }
}
