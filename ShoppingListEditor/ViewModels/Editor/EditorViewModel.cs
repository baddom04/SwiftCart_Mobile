﻿using DynamicData;
using ReactiveUI;
using ShoppingList.Core.Enums;
using ShoppingList.Shared;
using ShoppingList.Shared.Utils;
using ShoppingList.Shared.ViewModels;
using ShoppingListEditor.Model;
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

        public ReactiveCommand<LoggedInPages, Unit> ChangePageCommand { get; }
        public ReactiveCommand<Unit, Unit> SectionsPaneCommand { get; }
        public IReadOnlyCollection<SegmentType> SegmentTypes { get; }

        private SegmentType _selectedSegmentType = SegmentType.Shelf;
        public SegmentType SelectedSegmentType
        {
            get { return _selectedSegmentType; }
            set { this.RaiseAndSetIfChanged(ref _selectedSegmentType, value); }
        }

        public ObservableCollection<MapSegmentViewModel> MapSegments { get; } = [];
        public event Action? MapChanged;

        private readonly EditorModel _model;
        private readonly Action<LoggedInPages> _changePage;
        private readonly Action<bool> _showLoading;
        private readonly Action<NotificationType, string> _showNotification;
        public EditorViewModel(EditorModel model, Action<LoggedInPages> changePage, Action<bool> showLoading, Action<NotificationType, string> showNotification)
        {
            _model = model;
            _changePage = changePage;
            _showLoading = showLoading;
            _showNotification = showNotification;

            _model.MapChanged += OnMapChanged;

            ChangePageCommand = ReactiveCommand.Create<LoggedInPages>(lip => _changePage(lip));

            SegmentTypes = [.. Enum.GetValues(typeof(SegmentType)).Cast<SegmentType>()];

            _pages = new Dictionary<PanePage, ViewModelBase>()
            {
                { PanePage.Product, new ProductPaneViewModel() },
                { PanePage.Section, new SectionPaneViewModel(_model, _showLoading, _showNotification) },
            };

            SectionsPaneCommand = ReactiveCommand.Create(() => SetPaneContent(new SectionPaneViewModel(_model, _showLoading, _showNotification) { GoBack = () => IsPaneOpen = false }));
        }
        private void OnMapChanged()
        {
            if (_model.Store is null || _model.Store.Map is null) return;

            MapSegments.Clear();

            for (int y = 0; y < _model.Store.Map.YSize; y++)
            {
                for (int x = 0; x < _model.Store.Map.XSize; x++)
                {
                    MapSegments.Add(new MapSegmentViewModel(_model.Store.Map.MapSegments[y, x], _model, SetPaneContent, _showLoading, _showNotification));
                }
            }
            MapChanged?.Invoke();
        }

        public override void ChangeToDefaultPage()
        {
            throw new NotImplementedException();
        }

        private void SetPaneContent(ViewModelBase? viewModel)
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
