using DynamicData;
using ReactiveUI;
using ShoppingList.Core;
using ShoppingList.Model.Map;
using ShoppingList.Model.ShoppingList;
using ShoppingList.Model.Social;
using ShoppingList.Shared;
using ShoppingList.Shared.Model.Settings;
using ShoppingList.Shared.Utils;
using ShoppingList.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels.Map
{
    internal class StoreSettingsViewModel : ViewModelBase
    {
        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            private set { this.RaiseAndSetIfChanged(ref _isLoading, value); }
        }

        private bool _isGroceryListSearch;
        public bool IsGroceryListSearch
        {
            get { return _isGroceryListSearch; }
            set { this.RaiseAndSetIfChanged(ref _isGroceryListSearch, value); }
        }

        private string _searchInput = string.Empty;
        public string SearchInput
        {
            get { return _searchInput; }
            set { this.RaiseAndSetIfChanged(ref _searchInput, value); }
        }

        private bool _showSearchResults;
        public bool ShowSearchResults
        {
            get { return _showSearchResults; }
            private set { this.RaiseAndSetIfChanged(ref _showSearchResults, value); }
        }

        public ObservableCollection<ProductViewModel> SearchResults { get; } = [];
        public ObservableCollection<SectionViewModel> Sections { get; } = [];
        public ObservableCollection<Household> MyHouseholds { get; } = [];
        public bool HasHouseholds => MyHouseholds.Count != 0;
        public ObservableCollection<SectionViewModel> ShoppingItemSections { get; } = [];
        public bool HasShoppingItemSections => ShoppingItemSections.Count != 0;

        private Household? _selectedHousehold;
        public Household? SelectedHousehold
        {
            get { return _selectedHousehold; }
            set { this.RaiseAndSetIfChanged(ref _selectedHousehold, value); }
        }

        public List<ProductViewModel> AllProducts { get; } = [];
        public ReactiveCommand<Unit, Unit> GoBackCommand { get; }
        public ReactiveCommand<Unit, Unit> ClearCommand { get; }
        public ReactiveCommand<Unit, Unit> SearchCommand { get; }
        public ReactiveCommand<Section, Unit> SelectSectionCommand { get; }
        public ReactiveCommand<Unit, Unit> SearchShoppingListCommand { get; }

        private readonly MapModel _model;
        private readonly Action _goBack;
        private readonly Action<bool> _showLoading;
        private readonly UserAccountModel _account;
        private readonly Action<NotificationType, string> _showNotification;
        public StoreSettingsViewModel(UserAccountModel account, MapModel model, Action<bool> showLoading, Action goBack, Action<NotificationType, string> showNotification)
        {
            _account = account;
            _model = model;
            _showLoading = showLoading;
            _goBack = goBack;
            _showNotification = showNotification;
            GoBackCommand = ReactiveCommand.CreateFromTask(MarkAndGoBack);
            ClearCommand = ReactiveCommand.Create(ClearAllSelection);
            SearchCommand = ReactiveCommand.Create(OnSearch);
            SelectSectionCommand = ReactiveCommand.CreateFromTask<Section>(OnSectionSelected);
            SearchShoppingListCommand = ReactiveCommand.CreateFromTask(SearchShoppingList);

            MyHouseholds.CollectionChanged += (s, e) => this.RaisePropertyChanged(nameof(HasHouseholds));
            ShoppingItemSections.CollectionChanged += (s, e) => this.RaisePropertyChanged(nameof(HasShoppingItemSections));

            LoadData();
        }

        private async Task SearchShoppingList()
        {
            if (SelectedHousehold is null) return;

            IsLoading = true;
            try
            {
                IEnumerable<Grocery> items = await new ShoppingListModel(SelectedHousehold.Id).GetGroceriesAsync();
                ShoppingItemSections.Clear();
                ShoppingItemSections.AddRange(await GetShoppingItemSections(items));
            }
            catch (Exception ex)
            {
                string message = $"{StringProvider.GetString("ShoppingListQueryError")}{ex.Message}";
                _showNotification(NotificationType.Error, message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task<IEnumerable<SectionViewModel>> GetShoppingItemSections(IEnumerable<Grocery> items)
        {
            List<SectionViewModel> result = [];
            await Task.Run(() =>
            {
                foreach (Grocery item in items)
                {
                    List<ProductViewModel> products = Search(item.Name, 5);
                    result.Add(new SectionViewModel(new Section() { Name = item.Name }, products));
                }
            });
            return result;
        }

        public async Task GetMyHouseholdsAsync()
        {
            IsLoading = true;

            try
            {
                MyHouseholds.Clear();
                MyHouseholds.AddRange(await new MyHouseholdsModel().GetMyHouseholds(_account.User!.Id));
                SelectedHousehold = MyHouseholds.First();
            }
            catch (Exception ex)
            {
                string message = $"{StringProvider.GetString("MyHouseholdsQueryError")}{ex.Message}";
                _showNotification(NotificationType.Error, message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task OnSectionSelected(Section section)
        {
            ClearAllSelection();
            _model.SelectSection(section);
            await MarkAndGoBack();
            _model.UnSelectSection();
        }
        private async Task MarkAndGoBack()
        {
            _showLoading(true);
            await Task.Run(_model.MarkMapSegments);
            _showLoading(false);
            _goBack();
        }
        private List<ProductViewModel> Search(string searchText, int resultCount)
        {
            return AllProducts
                .Select(p => new
                {
                    Product = p,
                    Score = FuzzyMatcher.CommandScore(p.Product.Name, searchText) +
                    FuzzyMatcher.CommandScore(p.Product.Brand, searchText) +
                    (p.Product.Description is not null ? FuzzyMatcher.CommandScore(p.Product.Description!, searchText) : 0)
                })
                .Where(x => x.Score != 0)
                .OrderByDescending(x => x.Score)
                .Take(resultCount)
                .Select(x => x.Product)
                .ToList();
        }
        private async void OnSearch()
        {
            IsLoading = true;
            ShowSearchResults = SearchInput.Length != 0;
            SearchResults.Clear();
            SearchResults.AddRange(await Task.Run(() => Search(SearchInput.Trim(), 10)));
            IsLoading = false;
        }
        private void ClearAllSelection()
        {
            AllProducts.ForEach(p => p.IsSelected = false);
            _model.Clear();
        }
        private void LoadData()
        {
            _showLoading(true);
            InitializeData();
            _showLoading(false);
        }
        private void InitializeData()
        {
            Dictionary<int, List<ProductViewModel>> temp =
                _model.Store.Map!.Sections.ToDictionary(
                    section => section.Id,
                    section => new List<ProductViewModel>());

            List<ProductViewModel> misc = [];

            foreach (MapSegment segment in _model.Store.Map!.MapSegments)
            {
                foreach (Product product in segment.Products)
                {
                    ProductViewModel productViewModel = new(_model, product);
                    if (segment.SectionId.HasValue)
                        temp[segment.SectionId.Value].Add(productViewModel);
                    else
                        misc.Add(productViewModel);

                    AllProducts.Add(productViewModel);
                }
            }

            foreach (Section section in _model.Store.Map!.Sections)
            {
                Sections.Add(new SectionViewModel(section, temp[section.Id]));
            }
            Section miscSection = new()
            {
                Id = -1,
                Name = StringProvider.GetString("Miscellaneous"),
                MapId = _model.Store.Map.Id,
            };
            Sections.Add(new SectionViewModel(miscSection, misc));
        }
    }
}
