using DynamicData;
using ReactiveUI;
using ShoppingList.Core;
using ShoppingList.Core.Enums;
using ShoppingList.Model.Map;
using ShoppingList.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;

namespace ShoppingList.ViewModels.Map
{
    internal class MapViewModel : ViewModelBase
    {
        private bool _isPaneOpen;
        public bool IsPaneOpen
        {
            get { return _isPaneOpen; }
            private set { this.RaiseAndSetIfChanged(ref _isPaneOpen, value); }
        }

        public ReactiveCommand<Unit, Unit> GoBackCommand { get; }
        public ReactiveCommand<Unit, Unit> StoreSettingsPageCommand { get; }
        public ReactiveCommand<Unit, Unit> UnSelectSegmentCommand { get; }
        public string Name { get; }
        public IEnumerable<MapSegment> MapSegments { get; }
        public ObservableCollection<SegmentType> SegmentTypes { get; } = [];

        private MapSegment? _selectedMapSegment;
        public MapSegment? SelectedMapSegment
        {
            get { return _selectedMapSegment; }
            set { this.RaiseAndSetIfChanged(ref _selectedMapSegment, value); }
        }
        public ObservableCollection<ProductViewModel> SelectedProductsOnSegment { get; } = [];

        private readonly Action<MapPages> _changePage;
        private readonly Action<ViewModelBase> _changeToPage;
        private readonly MapModel _model;
        private readonly StoreSettingsViewModel _settings;
        public MapViewModel(MapModel model, Action<bool> showLoading, Action<ViewModelBase> changeToPage, Action<MapPages> changePage)
        {
            _model = model;
            _changeToPage = changeToPage;
            _changePage = changePage;
            Name = model.Store.Name;
            MapSegments = model.Store.Map!.MapSegments;
            _settings = new StoreSettingsViewModel(_model, showLoading, () => _changeToPage(this));

            SegmentTypes = [.. MapSegments.Select(segment => segment.Type).Distinct()];
            SegmentTypes.Remove(SegmentType.Outside);
            SegmentTypes.Remove(SegmentType.Empty);

            GoBackCommand = ReactiveCommand.Create(() => _changePage(MapPages.StoreList));
            UnSelectSegmentCommand = ReactiveCommand.Create(UnSelectSegment);
            StoreSettingsPageCommand = ReactiveCommand.Create(() => _changeToPage(_settings));

            this.WhenAnyValue(x => x.SelectedMapSegment).Subscribe(OnMapSegmentSelected);
        }

        private void UnSelectSegment()
        {
            IsPaneOpen = false;
            SelectedProductsOnSegment.Clear();
            SelectedMapSegment = null;
        }

        private void OnMapSegmentSelected(MapSegment? segment)
        {
            if (segment == null) return;
            SelectedProductsOnSegment.Clear();
            SelectedProductsOnSegment
                .AddRange(_settings.AllProducts
                    .Where(pvm => pvm.Product.MapSegmentId == segment.Id)
                    .OrderByDescending(pvm => pvm.IsSelected));
            IsPaneOpen = true;
        }
    }
}
