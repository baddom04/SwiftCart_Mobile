using DynamicData;
using ReactiveUI;
using ShoppingList.Core;
using ShoppingList.Model.Map;
using ShoppingList.Shared;
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
        public ObservableCollection<ProductViewModel> MiscProducts { get; } = [];
        public List<ProductViewModel> AllProducts { get; } = [];
        public ReactiveCommand<Unit, Unit> GoBackCommand { get; }
        public ReactiveCommand<Unit, Unit> ClearCommand { get; }
        public ReactiveCommand<Unit, Unit> SearchCommand { get; }
        public ReactiveCommand<Section, Unit> SelectSectionCommand { get; }

        private readonly MapModel _model;
        private readonly Action _goBack;
        private readonly Action<bool> _showLoading;
        public StoreSettingsViewModel(MapModel model, Action<bool> showLoading, Action goBack)
        {
            _model = model;
            _showLoading = showLoading;
            _goBack = goBack;
            GoBackCommand = ReactiveCommand.CreateFromTask(MarkAndGoBack);
            ClearCommand = ReactiveCommand.Create(ClearAllSelection);
            SearchCommand = ReactiveCommand.Create(OnSearch);
            SelectSectionCommand = ReactiveCommand.CreateFromTask<Section>(OnSectionSelected);

            LoadData();
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

        private IEnumerable<ProductViewModel> Search()
        {
            return AllProducts
                .Select(p => new
                {
                    Product = p,
                    Score = FuzzyMatcher.CommandScore(p.Product.Name, SearchInput) +
                    FuzzyMatcher.CommandScore(p.Product.Brand, SearchInput) +
                    FuzzyMatcher.CommandScore(p.Product.Description, SearchInput)
                })
                .Where(x => x.Score != 0)
                .OrderByDescending(x => x.Score)
                .Take(10)
                .Select(x => x.Product)
                .ToList();
        }

        private async void OnSearch()
        {
            IsLoading = true;
            ShowSearchResults = SearchInput.Length != 0;
            SearchResults.Clear();
            SearchResults.AddRange(await Task.Run(Search));
            IsLoading = false;
        }
        private void ClearAllSelection()
        {
            AllProducts.ForEach(p => p.IsSelected = false);
            _model.Clear();
        }

        public void LoadData()
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

            MiscProducts.AddRange(misc);
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
