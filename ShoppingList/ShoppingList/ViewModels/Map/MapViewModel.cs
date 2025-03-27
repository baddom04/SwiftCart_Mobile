using ReactiveUI;
using ShoppingList.Core;
using ShoppingList.Model.Map;
using System;
using System.Collections.Generic;
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
        public MapViewModel(MapModel model, Action<ViewModelBase> changeToPage, Action<MapPages> changePage)
        {
            _changeToPage = changeToPage;
            _changePage = changePage;
            Name = model.Store.Name;
            MapSegments = model.Store.Map!.MapSegments;
            GoBackCommand = ReactiveCommand.Create(() => _changePage(MapPages.StoreList));
        }
    }
}
