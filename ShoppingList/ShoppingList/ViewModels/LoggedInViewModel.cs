using ReactiveUI;
using ShoppingList.Model.Social;
using ShoppingList.Shared;
using ShoppingList.Shared.Model.Settings;
using ShoppingList.Shared.Utils;
using ShoppingList.Shared.ViewModels;
using ShoppingList.Utils;
using ShoppingList.ViewModels.Map;
using ShoppingList.ViewModels.Settings;
using ShoppingList.ViewModels.ShoppingList;
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
        private MainViewModelBase _currentPage;
        public MainViewModelBase CurrentPage
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
        public Dictionary<MenuIcon, MainViewModelBase> Menus { get; }

        private readonly UserAccountModel _userAccount;
        private readonly Action<NotificationType, string> _showNotification;
        private readonly Action<bool> _showLoading;
        public LoggedInViewModel(UserAccountModel userAccount, Action<MainPage> changePage, Action<bool> showLoading, Action<NotificationType, string> showNotification)
        {
            MyHouseholdsModel myHouseholds = new();
            Menus = new Dictionary<MenuIcon, MainViewModelBase>
            {
                { new MenuIcon("Map", "globe_regular"), new MainMapViewModel(userAccount, showLoading, showNotification) },
                { new MenuIcon("Shopping list", "cart_regular"), new MainGroceryPageViewModel(userAccount, myHouseholds, showNotification, showLoading) },
                { new MenuIcon("Social", "people_regular"), new MainSocialPanelViewModel(myHouseholds, userAccount, showNotification, showLoading) },
                { new MenuIcon("Settings", "settings_regular"), new MainSettingsViewModel(userAccount, showLoading, showNotification, changePage) },
            };

            MenuItems = [.. Menus.Select(item => item.Key)];
            _selectedMenuItem = MenuItems[0];
            _currentPage = Menus[_selectedMenuItem];
            _userAccount = userAccount;
            _showLoading = showLoading;
            _showNotification = showNotification;
        }

        public async Task GetUserAsync()
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
