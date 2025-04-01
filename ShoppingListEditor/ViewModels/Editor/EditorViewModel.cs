using DynamicData;
using ReactiveUI;
using ShoppingList.Core.Enums;
using ShoppingList.Shared;
using ShoppingList.Shared.ViewModels;
using ShoppingListEditor.Model;
using ShoppingListEditor.Model.Editables;
using ShoppingListEditor.ViewModels.Editor.Pane;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;

namespace ShoppingListEditor.ViewModels.Editor
{
    internal class EditorViewModel : MainViewModelBase<PanePage>
    {
        private bool _isPaneOpen;
        public bool IsPaneOpen
        {
            get { return _isPaneOpen; }
            private set { this.RaiseAndSetIfChanged(ref _isPaneOpen, value); }
        }

        public ReactiveCommand<Unit, Unit> ChangeSizesCommand { get; }
        public IReadOnlyCollection<SegmentType> SegmentTypes { get; }
        public SegmentType SelectedSegmentType { get; set; } = SegmentType.Shelf;

        public ObservableCollection<MapSegmentEditable> MapSegments { get; } = [];

        private readonly EditorModel _model;
        private readonly Action<LoggedInPages> _changePage;
        public EditorViewModel(EditorModel model, Action<LoggedInPages> changePage)
        {
            _model = model;
            _changePage = changePage;

            _model.MapChanged += OnMapChanged;

            ChangeSizesCommand = ReactiveCommand.Create(() => _changePage(LoggedInPages.Map));

            SegmentTypes = [.. Enum.GetValues(typeof(SegmentType)).Cast<SegmentType>()];

            _pages = new Dictionary<PanePage, ViewModelBase>()
            {
                { PanePage.Product, new ProductPaneViewModel() },
                { PanePage.Section, new SectionPaneViewModel() },
            };
        }
        private void OnMapChanged()
        {
            if (_model.Store is null || _model.Store.Map is null) return;

            MapSegments.Clear();

            for (int y = 0; y < _model.Store.Map.YSize; y++)
            {
                for (int x = 0; x < _model.Store.Map.YSize; x++)
                {
                    MapSegments.Add(_model.Store.Map.MapSegments[y, x]);
                }
            }
        }

        public override void ChangeToDefaultPage()
        {
            throw new NotImplementedException();
        }

        private void ChangeToPage(ViewModelBase? viewModel)
        {
            if (viewModel is null)
            {
                IsPaneOpen = false;
                return;
            }

            CurrentPage = viewModel;
            IsPaneOpen = true;
        }
    }
    internal enum PanePage
    {
        Section,
        Product
    }
}
