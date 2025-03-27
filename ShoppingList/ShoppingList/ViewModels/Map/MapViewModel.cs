using ReactiveUI;
using ShoppingList.Core;
using ShoppingList.Core.Enums;
using ShoppingList.Model.Map;
using ShoppingList.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
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
        private readonly Action<MapPages> _changePage;
        private readonly Action<ViewModelBase> _changeToPage;
        public ObservableCollection<SegmentType> SegmentTypes { get; }
        public MapViewModel(MapModel model, Action<ViewModelBase> changeToPage, Action<MapPages> changePage)
        {
            _changeToPage = changeToPage;
            _changePage = changePage;
            Name = model.Store.Name;
            MapSegments = model.Store.Map!.MapSegments;
            GoBackCommand = ReactiveCommand.Create(() => _changePage(MapPages.StoreList));
            SegmentTypes = [SegmentType.Entrance, SegmentType.Fridge, SegmentType.Shelf,
                SegmentType.Wall, SegmentType.CashRegister];
        }
    }
}
