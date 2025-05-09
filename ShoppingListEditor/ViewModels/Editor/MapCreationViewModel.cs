﻿using ReactiveUI;
using ShoppingListEditor.Model;
using ShoppingListEditor.Utils;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingListEditor.ViewModels.Editor
{
    internal class MapCreationViewModel : StorePropertyEditorViewModel
    {
        private string? _errorMessage;
        public string? ErrorMessage
        {
            get { return _errorMessage; }
            private set { this.RaiseAndSetIfChanged(ref _errorMessage, value); }
        }

        private int _xSize = 10;
        public int XSize
        {
            get { return _xSize; }
            set { this.RaiseAndSetIfChanged(ref _xSize, value); }
        }

        private int _ySize = 10;
        public int YSize
        {
            get { return _ySize; }
            set { this.RaiseAndSetIfChanged(ref _ySize, value); }
        }

        public override bool IsUpdating => GetIsUpdating();
        public ReactiveCommand<Unit, Unit> SetMapDimensionsCommand { get; }

        private readonly Action<LoggedInPages> _changePage;
        private readonly Action<bool> _showLoading;
        public MapCreationViewModel(EditorModel model, Action<LoggedInPages> changePage, Action<bool> showLoading) : base(model, changePage)
        {
            _changePage = changePage;
            _showLoading = showLoading;
            SetMapDimensionsCommand = ReactiveCommand.CreateFromTask(SetMapDimensionsAsync);

            _model.MapChanged += OnMapChanged;
        }
        private bool GetIsUpdating()
        {
            if (_model.Store is null) return false;
            else return _model.Store.Map is not null;
        }
        private void OnMapChanged()
        {
            if (_model.Store is null || _model.Store.Map is null) return;

            XSize = _model.Store.Map.XSize;
            YSize = _model.Store.Map.YSize;
        }

        private async Task SetMapDimensionsAsync()
        {
            _showLoading(true);
            try
            {
                if(!IsUpdating)
                    await _model.CreateMapAsync(XSize, YSize);
                else
                    await _model.UpdateMapAsync(XSize, YSize);

                _changePage(LoggedInPages.Editor);
                ErrorMessage = null;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"{StringProvider.GetString("MapCreationError")}{ex.Message}";
            }
            finally
            {
                _showLoading(false);
            }
        }
    }
}
