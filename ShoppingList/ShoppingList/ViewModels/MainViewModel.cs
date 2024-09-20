using ReactiveUI;
using ShoppingList.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ShoppingList.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentPage;
        public ViewModelBase CurrentPage
        {
            get { return _currentPage; }
            set { this.RaiseAndSetIfChanged(ref _currentPage, value); }
        }

        private MenuItem _selectedMenuItem;
        public MenuItem SelectedMenuItem
        {
            get { return _selectedMenuItem; }
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedMenuItem, value);
                // TODO: Refactor (fucking delegate type error)
                CurrentPage = Menus[value];
            }
        }

        public ObservableCollection<MenuItem> MenuItems { get; }

        public Dictionary<MenuItem, ViewModelBase> Menus { get; } = new Dictionary<MenuItem, ViewModelBase>
        {
            //{ new MenuItem("Home", "globe_regular"), new MainViewModel() },
            { new MenuItem("Shopping list", "cart_regular"), new GroceryListViewModel() },
            { new MenuItem("Social", "people_regular"), new SocialPanelViewModel() },
            { new MenuItem("Settings", "settings_regular"), new SettingsViewModel() },
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
