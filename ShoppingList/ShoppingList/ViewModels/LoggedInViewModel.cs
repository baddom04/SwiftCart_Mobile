using ReactiveUI;
using ShoppingList.Model.Models;
using ShoppingList.Utils;
using ShoppingList.ViewModels.GroceryList;
using ShoppingList.ViewModels.Map;
using ShoppingList.ViewModels.Settings;
using ShoppingList.ViewModels.Social;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ShoppingList.ViewModels
{
    internal class LoggedInViewModel : ViewModelBase
    {
        private ViewModelBase _currentPage;
        public ViewModelBase CurrentPage
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
                // TODO: Refactor (fucking delegate type error)
                CurrentPage = Menus[value];
            }
        }
        public ObservableCollection<MenuIcon> MenuItems { get; }

        public Dictionary<MenuIcon, ViewModelBase> Menus { get; }

        private readonly UserAccountModel _userAccount;
        public LoggedInViewModel(UserAccountModel userAccount, Action<Page> changePage, Action<bool> showLoading, Action<NotificationType, string> showNotification)
        {

            Menus = new Dictionary<MenuIcon, ViewModelBase>
            {
                { new MenuIcon("Map", "globe_regular"), new MapViewModel() },
                { new MenuIcon("Shopping list", "cart_regular"), new GroceryListViewModel() },
                { new MenuIcon("Social", "people_regular"), new SocialPanelViewModel() },
                { new MenuIcon("Settings", "settings_regular"), new SettingsViewModel(userAccount, changePage, showLoading, showNotification) },
            };

            MenuItems = [.. Menus.Select(item => item.Key)];
            _selectedMenuItem = MenuItems[3];
            _currentPage = Menus[_selectedMenuItem];
            _userAccount = userAccount;

            //this.WhenAnyValue(x => x.SelectedMenuItem)
            //    .Where(selectedItem => selectedItem is not null)
            //    .Subscribe(selectedItem => CurrentPage = Menus[selectedItem]);
        }
    }
}
