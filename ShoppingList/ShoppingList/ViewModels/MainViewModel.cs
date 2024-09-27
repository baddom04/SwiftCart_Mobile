using Avalonia.Controls;
using ReactiveUI;
using ShoppingList.Models;
using ShoppingList.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

namespace ShoppingList.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private UserControl _currentPage;
        public UserControl CurrentPage
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

        public Dictionary<MenuIcon, UserControl> Menus { get; } = new Dictionary<MenuIcon, UserControl>
        {
            { new MenuIcon("Map", "globe_regular"), new MapView() },
            { new MenuIcon("Shopping list", "cart_regular"), new GroceryListView() },
            { new MenuIcon("Social", "people_regular"), new SocialPanelView() },
            { new MenuIcon("Settings", "settings_regular"), new SettingsView() },
        };

        public MainViewModel()
        {
            MenuItems = [.. Menus.Select(item => item.Key)];
            _selectedMenuItem = MenuItems[0];
            _currentPage = Menus[_selectedMenuItem];

            //this.WhenAnyValue(x => x.SelectedMenuItem)
            //    .Where(selectedItem => selectedItem is not null)
            //    .Subscribe(selectedItem => CurrentPage = Menus[selectedItem]);
        }
    }
}
