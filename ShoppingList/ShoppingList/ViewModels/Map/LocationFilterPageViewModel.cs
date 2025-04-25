using ReactiveUI;
using ShoppingList.Core;
using ShoppingList.Model.Map;
using ShoppingList.Shared;
using ShoppingList.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels.Map
{
    internal class LocationFilterPageViewModel : ViewModelBase
    {
        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            private set { this.RaiseAndSetIfChanged(ref _isLoading, value); }
        }

        public ReactiveCommand<Unit, Unit> SearchCommand { get; }
        public ReactiveCommand<Unit, Unit> GoBackCommand { get; }
        public ObservableCollection<LocationFilterViewModel> LocationFilters { get; } = [];

        private readonly Action<MapPage> _changePage;
        private readonly Action<LocationFilter> _setLocationFilter;
        private readonly Dictionary<LocationProperty, LocationFilterViewModel> _locationsFilters = [];
        public LocationFilterPageViewModel(LocationFilterModel model, Action<MapPage> changePage, Action<NotificationType, string> showNotification, Action<LocationFilter> setLocationFilter)
        {
            _changePage = changePage;
            _setLocationFilter = setLocationFilter;

            SearchCommand = ReactiveCommand.Create(() => SetLocationFilter(new LocationFilter()
            {
                Country = _locationsFilters[LocationProperty.Country].SearchResult ?? string.Empty,
                City = _locationsFilters[LocationProperty.City].SearchResult ?? string.Empty,
                Street = _locationsFilters[LocationProperty.Street].SearchResult ?? string.Empty,
                Detail = _locationsFilters[LocationProperty.Details].SearchResult ?? string.Empty,
            }));
            GoBackCommand = ReactiveCommand.Create(() => SetLocationFilter(new LocationFilter()));

            foreach (LocationProperty type in Enum.GetValues(typeof(LocationProperty)))
            {
                var filterViewModel = new LocationFilterViewModel(model.LocationFilters[type], type, ShowLoading, showNotification);
                LocationFilters.Add(filterViewModel);
                _locationsFilters.Add(type, filterViewModel);
            }
        }
        private void ShowLoading(bool isLoading)
        {
            IsLoading = isLoading;
        }
        public async Task GetPossibleCountriesAsync()
        {
            await _locationsFilters[LocationProperty.Country].SearchAsync();
        }
        public void SetLocationFilter(LocationFilter filter)
        {
            _setLocationFilter(filter);
            _changePage(MapPage.StoreList);
        }
    }
}
