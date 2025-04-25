using DynamicData;
using ReactiveUI;
using ShoppingList.Core;
using ShoppingList.Core.Enums;
using ShoppingList.Model.Map;
using ShoppingList.Shared;
using ShoppingList.Shared.Model.Settings;
using ShoppingList.Shared.Utils;
using ShoppingList.Utils;
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

        private string? _sectionName;
        public string? SectionName
        {
            get { return _sectionName; }
            private set { this.RaiseAndSetIfChanged(ref _sectionName, value); }
        }

        private MapSegment? _selectedMapSegment;
        public MapSegment? SelectedMapSegment
        {
            get { return _selectedMapSegment; }
            set { this.RaiseAndSetIfChanged(ref _selectedMapSegment, value); }
        }

        public ReactiveCommand<Unit, Unit> GoBackCommand { get; }
        public ReactiveCommand<Unit, Unit> StoreSettingsPageCommand { get; }
        public ReactiveCommand<Unit, Unit> UnSelectSegmentCommand { get; }
        public string Name { get; }
        public IEnumerable<MapSegment> MapSegments { get; }
        public ObservableCollection<SegmentType> SegmentTypes { get; } = [];
        public ObservableCollection<ProductViewModel> SelectedProductsOnSegment { get; } = [];

        private readonly Action<MapPage> _changePage;
        private readonly Action<ViewModelBase> _changeToPage;
        private readonly MapModel _model;
        private readonly StoreSettingsViewModel _settings;
        public MapViewModel(UserAccountModel account, MapModel model, Action<bool> showLoading, Action<ViewModelBase> changeToPage, Action<MapPage> changePage, Action<NotificationType, string> showNotification)
        {
            _model = model;
            _changeToPage = changeToPage;
            _changePage = changePage;
            Name = model.Store.Name;
            MapSegments = model.Store.Map!.MapSegments;
            _settings = new StoreSettingsViewModel(account, _model, showLoading, () => _changeToPage(this), showNotification);

            SegmentTypes = [.. MapSegments.Select(segment => segment.Type).Distinct()];
            SegmentTypes.Remove(SegmentType.Empty);

            GoBackCommand = ReactiveCommand.Create(() => _changePage(MapPage.StoreList));
            UnSelectSegmentCommand = ReactiveCommand.Create(UnSelectSegment);
            StoreSettingsPageCommand = ReactiveCommand.Create(() => _changeToPage(_settings));

            this.WhenAnyValue(x => x.SelectedMapSegment).Subscribe(OnMapSegmentSelected);
        }

        private void UnSelectSegment()
        {
            IsPaneOpen = false;
            SelectedProductsOnSegment.Clear();
            SelectedMapSegment = null;
            SectionName = null;
        }

        private void OnMapSegmentSelected(MapSegment? segment)
        {
            if (segment == null) return;
            SelectedProductsOnSegment.Clear();
            SelectedProductsOnSegment
                .AddRange(_settings.AllProducts
                    .Where(pvm => pvm.Product.MapSegmentId == segment.Id)
                    .OrderByDescending(pvm => pvm.IsSelected));

            SectionName = _model.Store.Map!.Sections.FirstOrDefault(s => s.Id == segment.SectionId)?.Name
                ?? StringProvider.GetString("None");

            if(SelectedProductsOnSegment.Count != 0)
                IsPaneOpen = true;
        }
    }
}
