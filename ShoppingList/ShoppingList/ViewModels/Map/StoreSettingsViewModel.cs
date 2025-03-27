using ReactiveUI;
using ShoppingList.Core;
using ShoppingList.Model.Map;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive;
using System.Timers;
using System.Threading.Tasks;
using ShoppingList.Utils;
using DynamicData;
using Avalonia.Threading;

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
            set
            {
                this.RaiseAndSetIfChanged(ref _searchInput, value);
                this.RaisePropertyChanged(nameof(ShowSearchResults));
            }
        }

        public bool ShowSearchResults => SearchInput.Length != 0;
        public ObservableCollection<ProductViewModel> SearchResults { get; } = [];
        public ObservableCollection<SectionViewModel> Sections { get; } = [];
        public SectionViewModel? SelectedSection { get; set; }
        public ObservableCollection<ProductViewModel> MiscProducts { get; } = [];
        public ObservableCollection<ProductViewModel> SelectedMiscProducts { get; } = [];
        public ReactiveCommand<Unit, Task> GoBackCommand { get; }
        public ReactiveCommand<Unit, Unit> ClearCommand { get; }
        public ReactiveCommand<Unit, Unit> SearchCommand { get; }

        private readonly MapModel _model;
        private readonly Action _goBack;
        private readonly List<Product> _allProducts = [];
        private readonly Action<bool> _showLoading;
        public StoreSettingsViewModel(MapModel model, Action<bool> showLoading, Action goBack)
        {
            _model = model;
            _showLoading = showLoading;
            _goBack = goBack;
            GoBackCommand = ReactiveCommand.Create(async () =>
            {
                _showLoading(true);
                await Task.Run(_model.MarkMapSegments);
                _showLoading(false);
                _goBack();
            });
            ClearCommand = ReactiveCommand.CreateFromTask(ClearAllSelection);
            SearchCommand = ReactiveCommand.Create(OnSearch);

            SelectedMiscProducts.CollectionChanged += MiscProducts_CollectionChanged;

            this.WhenAnyValue(x => x.SelectedSection).Subscribe(OnSectionSelected);
        }

        private IEnumerable<ProductViewModel> Search()
        {
            return _allProducts
                .Select(p => new { Product = p, Score = FuzzyMatcher.CommandScore(p.Name, SearchInput) })
                .Where(x => x.Score != 0)
                .OrderByDescending(x => x.Score)
                .Take(10)
                .Select(x => new ProductViewModel(x.Product))
                .ToList();
        }

        private async void OnSearch()
        {
            IsLoading = true;
            SearchResults.Clear();
            SearchResults.AddRange(await Task.Run(Search));
            IsLoading = false;
        }

        private void MiscProducts_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems is null) break;
                    foreach (ProductViewModel newItem in e.NewItems)
                    {
                        _model.Select(newItem.Product);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems is null) break;
                    foreach (ProductViewModel oldItem in e.OldItems)
                    {
                        _model.UnSelect(oldItem.Product);
                    }
                    break;
                default:
                    break;
            }
        }

        private async void OnSectionSelected(SectionViewModel? sectionViewModel)
        {
            if (sectionViewModel == null) return;

            _showLoading(true);
            await ClearAllSelection();
            _model.SelectSection(sectionViewModel.Section);

            _showLoading(true);
            await Task.Run(_model.MarkMapSegments);
            _showLoading(false);
            _goBack();
        }
        private async Task ClearAllSelection()
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                SelectedMiscProducts.Clear();
                Sections.ToList().ForEach(s => s.SelectedProducts.Clear());
                _model.Clear();
            });
        }

        public async Task LoadData()
        {
            _showLoading(true);
            await Task.Run(InitializeData);
            _showLoading(false);
        }
        private async Task InitializeData()
        {
            Dictionary<int, List<Product>> temp =
                _model.Store.Map!.Sections.ToDictionary(
                    section => section.Id,
                    section => new List<Product>());

            List<ProductViewModel> temp2 = [];

            foreach (MapSegment segment in _model.Store.Map!.MapSegments)
            {
                foreach (Product product in segment.Products)
                {
                    if (segment.SectionId.HasValue)
                        temp[segment.SectionId.Value].Add(product);
                    else
                        temp2.Add(new ProductViewModel(product));

                    _allProducts.Add(product);
                }
            }
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                MiscProducts.AddRange(temp2);
                foreach (Section section in _model.Store.Map!.Sections)
                {
                    Sections.Add(new SectionViewModel(_model, section, temp[section.Id]));
                }
            });
        }
    }
}
