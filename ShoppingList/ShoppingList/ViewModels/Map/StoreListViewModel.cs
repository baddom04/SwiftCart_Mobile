using DynamicData;
using ReactiveUI;
using ShoppingList.Model.Map;
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
        private readonly Action<ViewModelBase> _changePage;
        private readonly StoreListModel _model;

        public StoreListViewModel(StoreListModel model, Action<NotificationType, string> showNotification, Action<ViewModelBase> changePage)
        {
            _model = model;
            _showNotification = showNotification;
            _changePage = changePage;
            SearchCommand = ReactiveCommand.CreateFromTask(() => Search());

            TurnPageForwardCommand = ReactiveCommand.CreateFromTask(() => Search(Page + 1),
            this.WhenAnyValue(x => x.Page, x => x.MaxPage, (page, maxPage) => page != maxPage));

            TurnPageBackwardCommand = ReactiveCommand.CreateFromTask(() => Search(Page - 1),
                this.WhenAnyValue(x => x.Page, page => page != 1));

            Stores.CollectionChanged += (s, e) => this.RaisePropertyChanged(nameof(EmptyStores));
        }

        public async Task Search(int page = 1)
        {
            IsLoading = true;
            Page = page;

            try
            {
                Stores.Clear();
                Stores.AddRange((await _model.GetStoresAsync(SearchInput, Page))
                    .Select(store => new StoreListItemViewModel(new StoreListItemModel(store), _showNotification, _changePage)));

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
