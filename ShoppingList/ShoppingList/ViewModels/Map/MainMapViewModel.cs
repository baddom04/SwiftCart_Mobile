using ShoppingList.Model.Map;
using ShoppingList.Utils;
using ShoppingList.ViewModels.Shared;
using System;

namespace ShoppingList.ViewModels.Map
{
    internal class MainMapViewModel : MainViewModelBase<MapPages>
    {
        public override void ChangeToDefaultPage()
        {
            ChangePage(MapPages.StoreList);
        }

        public MainMapViewModel(Action<NotificationType, string> showNotification)
        {
            _pages = new()
            {
                { MapPages.StoreList, new StoreListViewModel(new StoreListModel(), showNotification, ChangeToPage, ChangePage) },
                { MapPages.StoreSettings, new StoreSettingsViewModel() },
            };

            _currentPage = _pages[MapPages.StoreList];
        }
        private void ChangeToPage(ViewModelBase page)
        {
            CurrentPage = page;
        }
    }
    internal enum MapPages
    {
        StoreList,
        Map,
        StoreSettings
    }
}