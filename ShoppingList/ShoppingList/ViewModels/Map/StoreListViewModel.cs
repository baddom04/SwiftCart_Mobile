using DynamicData;
using ReactiveUI;
using ShoppingList.Core;
using ShoppingList.Model.Map;
using ShoppingList.Shared;
using ShoppingList.Shared.Model.Settings;
using ShoppingList.Shared.Utils;
using ShoppingList.Utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels.Map
{
    internal class StoreListViewModel : ViewModelBase
    {
        public string SearchInput { get; set; } = string.Empty;
        public ObservableCollection<StoreListItemViewModel> Stores { get; } = [];
        public bool EmptyStores => Stores.Count == 0;
        public ReactiveCommand<Unit, Unit> SearchCommand { get; }
        public ReactiveCommand<Unit, Unit> TurnPageForwardCommand { get; }
        public ReactiveCommand<Unit, Unit> TurnPageBackwardCommand { get; }
        public ReactiveCommand<Unit, Unit> LocationFilterPageCommand { get; }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            private set { this.RaiseAndSetIfChanged(ref _isLoading, value); }
        }

        private int _page = 1;
        public int Page
        {
            get { return _page; }
            private set { this.RaiseAndSetIfChanged(ref _page, value); }
        }

        private int _maxPage = 1;
        public int MaxPage
        {
            get { return _maxPage; }
            private set { this.RaiseAndSetIfChanged(ref _maxPage, value); }
        }

        private readonly Action<NotificationType, string> _showNotification;
        private readonly Action<ViewModelBase> _changeToPage;
        private readonly Action<MapPage> _changePage;
        private readonly StoreListModel _model;
        private readonly Action<bool> _showLoading;
        private readonly UserAccountModel _account;
        public Action<LocationFilter> SetLocationFilter { get; }
        public LocationFilter LocationFilter { get; private set; }
        public StoreListViewModel(UserAccountModel account, StoreListModel model, Action<bool> showLoading, Action<NotificationType, string> showNotification, Action<ViewModelBase> changeToPage, Action<MapPage> changePage)
        {
            _account = account;
            _model = model;
            _showLoading = showLoading;
            _showNotification = showNotification;
            _changeToPage = changeToPage;
            _changePage = changePage;
            SearchCommand = ReactiveCommand.CreateFromTask(() => SearchAsync());
            LocationFilterPageCommand = ReactiveCommand.Create(() => changePage(MapPage.LocationFilter));

            TurnPageForwardCommand = ReactiveCommand.CreateFromTask(() => SearchAsync(Page + 1),
            this.WhenAnyValue(x => x.Page, x => x.MaxPage, (page, maxPage) => page != maxPage));

            TurnPageBackwardCommand = ReactiveCommand.CreateFromTask(() => SearchAsync(Page - 1),
                this.WhenAnyValue(x => x.Page, page => page != 1));

            Stores.CollectionChanged += (s, e) => this.RaisePropertyChanged(nameof(EmptyStores));

            LocationFilter = new LocationFilter();
            SetLocationFilter = (lf) => LocationFilter = lf;
        }

        public async Task SearchAsync(int page = 1)
        {
            IsLoading = true;
            Page = page;

            try
            {
                Stores.Clear();
                Stores.AddRange((await _model.GetStoresAsync(SearchInput.Trim(), Page, LocationFilter))
                    .Select(store => new StoreListItemViewModel(_account, new StoreListItemModel(store), _showLoading, _showNotification, _changeToPage, _changePage)));

                MaxPage = _model.MaxPages;
            }
            catch (Exception ex)
            {
                string message = $"{StringProvider.GetString("SearchStoresError")}{ex.Message}";
                _showNotification(NotificationType.Error, message);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
