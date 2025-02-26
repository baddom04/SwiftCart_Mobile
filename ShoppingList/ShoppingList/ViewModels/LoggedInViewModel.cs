using ReactiveUI;
using ShoppingList.Model.Models;
using ShoppingList.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ShoppingList.ViewModels
{
    public class LoggedInViewModel : ViewModelBase
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

        public Dictionary<MenuIcon, ViewModelBase> Menus { get; } = new Dictionary<MenuIcon, ViewModelBase>
        {
            { new MenuIcon("Map", "globe_regular"), new MapViewModel() },
            { new MenuIcon("Shopping list", "cart_regular"), new GroceryListViewModel() },
            { new MenuIcon("Social", "people_regular"), new SocialPanelViewModel() },
            { new MenuIcon("Settings", "settings_regular"), new SettingsViewModel() },
        };

        private readonly UserAccountModel _accountModel;
        public LoggedInViewModel(UserAccountModel accountModel)
        {
            MenuItems = [.. Menus.Select(item => item.Key)];
            _selectedMenuItem = MenuItems[0];
            _currentPage = Menus[_selectedMenuItem];
            _accountModel = accountModel;

            //this.WhenAnyValue(x => x.SelectedMenuItem)
            //    .Where(selectedItem => selectedItem is not null)
            //    .Subscribe(selectedItem => CurrentPage = Menus[selectedItem]);
        }
    }
}
