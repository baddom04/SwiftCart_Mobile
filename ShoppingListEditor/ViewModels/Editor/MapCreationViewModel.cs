using ReactiveUI;
using ShoppingList.Shared;
using ShoppingListEditor.Model;
using ShoppingListEditor.Utils;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingListEditor.ViewModels.Editor
{
    internal class MapCreationViewModel : ViewModelBase
    {
        public bool IsUpdating => GetIsUpdating();
        private bool GetIsUpdating()
        {
            if (_model.Store is null) return false;
            else return _model.Store.Map is not null;
        }

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
        public ReactiveCommand<Unit, Unit> SetMapDimensionsCommand { get; }
        public ReactiveCommand<Unit, Unit> GoBackCommand { get; }

        private readonly EditorModel _model;
        private readonly Action<LoggedInPages> _changePage;
        private readonly Action<bool> _showLoading;
        public MapCreationViewModel(EditorModel model, Action<LoggedInPages> changePage, Action<bool> showLoading)
        {
            _model = model;
            _changePage = changePage;
            _showLoading = showLoading;
            SetMapDimensionsCommand = ReactiveCommand.CreateFromTask(SetMapDimensions);
            GoBackCommand = ReactiveCommand.Create(() => _changePage(LoggedInPages.Editor),
                this.WhenAnyValue(x => x.IsUpdating, isUpdating => isUpdating == true));

            _model.MapChanged += OnMapChanged;
        }

        private void OnMapChanged()
        {
            if (_model.Store is null || _model.Store.Map is null) return;

            XSize = _model.Store.Map.XSize;
            YSize = _model.Store.Map.YSize;
        }

        private async Task SetMapDimensions()
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
