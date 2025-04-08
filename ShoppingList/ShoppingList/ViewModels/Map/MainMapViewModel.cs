using ShoppingList.Model.Map;
using ShoppingList.Shared;
using ShoppingList.Shared.Utils;
using ShoppingList.Shared.ViewModels;
using System;

namespace ShoppingList.ViewModels.Map
{
    internal class MainMapViewModel : MainViewModelBase<MapPage>
    {
        public override void ChangeToDefaultPage()
        {
            ChangePage(MapPage.StoreList);
        }

        public MainMapViewModel(Action<bool> showLoading, Action<NotificationType, string> showNotification)
        {
            var storeListViewModel = new StoreListViewModel(new StoreListModel(), showLoading, showNotification, ChangeToPage, ChangePage);
            _pages = new()
            {
                { MapPage.StoreList, storeListViewModel },
                { MapPage.LocationFilter, new LocationFilterPageViewModel(new LocationFilterModel(), ChangePage, showNotification, storeListViewModel.SetLocationFilter) }
            };

            _currentPage = _pages[MapPage.StoreList];
        }
        private void ChangeToPage(ViewModelBase page)
        {
            CurrentPage = page;
        }
    }
    internal enum MapPage
    {
        StoreList,
        Map,
        StoreSettings,
        LocationFilter
    }
}