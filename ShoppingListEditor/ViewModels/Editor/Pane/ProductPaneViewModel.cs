using ReactiveUI;
using ShoppingList.Shared.Utils;
using ShoppingListEditor.Model;
using ShoppingListEditor.Model.Editables;
using ShoppingListEditor.Utils;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingListEditor.ViewModels.Editor.Pane
{
    internal class ProductPaneViewModel : PanePageViewModel
    {
        private string? _errorMessage;
        public string? ErrorMessage
        {
            get { return _errorMessage; }
            private set { this.RaiseAndSetIfChanged(ref _errorMessage, value); }
        }

        public bool IsUpdating { get; private set; }
        public string NameInput { get; set; } = string.Empty;
        public string DescriptionInput { get; set; } = string.Empty;
        public string BrandInput { get; set; } = string.Empty;
        public string PriceInput { get; set; } = string.Empty;

        public ReactiveCommand<Unit, Unit> CreateCommand { get; }
        public ReactiveCommand<Unit, Unit> UpdateCommand { get; }
        public ReactiveCommand<Unit, Unit> GoBackCommand { get; }

        private readonly EditorModel _model;
        private readonly MapSegmentEditable _segment;
        private readonly Action<bool> _showLoading;
        private readonly Action<NotificationType, string> _showNotification;
        private readonly int? _productId;
        public ProductPaneViewModel(EditorModel model, MapSegmentEditable segment, Action<bool> showLoading, Action<NotificationType, string> showNotification)
        {
            _model = model;
            _segment = segment;
            _showLoading = showLoading;
            _showNotification = showNotification;

            CreateCommand = ReactiveCommand.CreateFromTask(CreateProductAsync);
            UpdateCommand = ReactiveCommand.CreateFromTask(UpdateProductAsync);
            GoBackCommand = ReactiveCommand.Create(() => GoBack!());
        }

        public ProductPaneViewModel(EditorModel model, MapSegmentEditable segment, ProductEditable product, Action<bool> showLoading, Action<NotificationType, string> showNotification)
        {
            _model = model;
            _segment = segment;
            _showLoading = showLoading;
            _showNotification = showNotification;

            IsUpdating = true;
            NameInput = product.Name;
            DescriptionInput = product.Description;
            BrandInput = product.Brand;
            PriceInput = product.Price.ToString();
            _productId = product.Id;

            CreateCommand = ReactiveCommand.CreateFromTask(CreateProductAsync);
            UpdateCommand = ReactiveCommand.CreateFromTask(UpdateProductAsync);
            GoBackCommand = ReactiveCommand.Create(() => GoBack!());
        }

        private async Task CreateProductAsync()
        {
            if (!Validate()) return;

            _showLoading(true);
            try
            {
                await _model.CreateProductAsync(_segment, NameInput.Trim(), DescriptionInput.Trim(), BrandInput.Trim(), PriceInput.Trim());
                GoBack!();
            }
            catch (Exception ex)
            {
                string msg = $"{StringProvider.GetString("ProductUploadError")}{ex.Message}";
                _showNotification(NotificationType.Error, msg);
            }
            finally { _showLoading(false); }
        }
        private async Task UpdateProductAsync()
        {
            if (!_productId.HasValue || !Validate()) return;

            _showLoading(true);
            try
            {
                await _model.UpdateProductAsync(_segment, _productId.Value, NameInput.Trim(), DescriptionInput.Trim(), BrandInput.Trim(), PriceInput.Trim());
                GoBack!();
            }
            catch (Exception ex)
            {
                string msg = $"{StringProvider.GetString("ProductUploadError")}{ex.Message}";
                _showNotification(NotificationType.Error, msg);
            }
            finally { _showLoading(false); }
        }

        private bool Validate()
        {
            if(!ValidateEmpty(NameInput, nameof(NameInput).Replace("Input", "")) ||
               !ValidateEmpty(BrandInput, nameof(BrandInput).Replace("Input", "")) ||
               !ValidateEmpty(DescriptionInput, nameof(DescriptionInput).Replace("Input", "")) ||
               !ValidateEmpty(PriceInput, nameof(PriceInput).Replace("Input", "")))
            {
                return false;
            }
            if(NameInput.Trim().Length > 20)
            {
                ErrorMessage = StringProvider.GetString("ProductNameTooLong");
                return false;
            }
            if(BrandInput.Trim().Length > 20)
            {
                ErrorMessage = StringProvider.GetString("BrandTooLong");
                return false;
            }
            if(DescriptionInput.Trim().Length > 255)
            {
                ErrorMessage = StringProvider.GetString("DescriptionTooLong");
                return false;
            }
            if(!uint.TryParse(PriceInput.Trim(), out var _))
            {
                ErrorMessage = StringProvider.GetString("IncorrectPriceFormat");
                return false;
            }
            return true;
        }
        private bool ValidateEmpty(string str, string key)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                ErrorMessage = $"{StringProvider.GetString("SmthMissing")} {StringProvider.GetString(key)}.";
                return false;
            }

            return true;
        }
    }
}
