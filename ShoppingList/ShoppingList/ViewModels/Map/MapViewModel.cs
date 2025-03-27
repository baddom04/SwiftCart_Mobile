using ReactiveUI;
using ShoppingList.Core;
using ShoppingList.Core.Enums;
using ShoppingList.Model.Map;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;

namespace ShoppingList.ViewModels.Map
{
    internal class MapViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> GoBackCommand { get; }
        public ReactiveCommand<Unit, Unit> StoreSettingsPageCommand { get; }
        public string Name { get; }
        public IEnumerable<MapSegment> MapSegments { get; }
        public ObservableCollection<SegmentType> SegmentTypes { get; } = [];

        private readonly Action<MapPages> _changePage;
        private readonly Action<ViewModelBase> _changeToPage;
        private readonly MapModel _model;
        private StoreSettingsViewModel? _settings;
        public MapViewModel(MapModel model, Action<bool> showLoading, Action<ViewModelBase> changeToPage, Action<MapPages> changePage)
        {
            _model = model;
            _changeToPage = changeToPage;
            _changePage = changePage;
            Name = model.Store.Name;
            MapSegments = model.Store.Map!.MapSegments;

            SegmentTypes = [.. MapSegments.Select(segment => segment.Type).Distinct()];
            SegmentTypes.Remove(SegmentType.Outside);
            SegmentTypes.Remove(SegmentType.Empty);

            GoBackCommand = ReactiveCommand.Create(() => _changePage(MapPages.StoreList));

            StoreSettingsPageCommand = ReactiveCommand.Create(() =>
            {
                _settings ??= new StoreSettingsViewModel(_model, showLoading, () => _changeToPage(this));
                _changeToPage(_settings);
            });
        }
    }
}
