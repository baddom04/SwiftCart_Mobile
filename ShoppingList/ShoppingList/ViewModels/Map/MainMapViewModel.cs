using ShoppingList.Model.Map;
using ShoppingList.Utils;
using ShoppingList.Shared.ViewModels;
using System;
using ShoppingList.Shared;

namespace ShoppingList.ViewModels.Map
{
    internal class MainMapViewModel : MainViewModelBase<MapPages>
    {
        public override void ChangeToDefaultPage()
        {
            ChangePage(MapPages.StoreList);
        }

        public MainMapViewModel(Action<bool> showLoading, Action<NotificationType, string> showNotification)
        {
            _pages = new()
            {
                { MapPages.StoreList, new StoreListViewModel(new StoreListModel(), showLoading, showNotification, ChangeToPage, ChangePage) },
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