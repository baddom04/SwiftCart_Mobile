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
        public bool IsUpdating { get; private set; }
        public string NameInput { get; set; } = string.Empty;
        public string DescriptionInput { get; set; } = string.Empty;
        public string BrandInput { get; set; } = string.Empty;
        public string PriceInput { get; set; } = string.Empty;

        public ReactiveCommand<Unit, Unit> CreateCommand { get; }
        public ReactiveCommand<Unit, Unit> UpdateCommand { get; }

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
        }

        private async Task CreateProductAsync()
        {
            _showLoading(true);
            try
            {
                await _model.CreateProductAsync(_segment, NameInput, DescriptionInput, BrandInput, PriceInput);
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
            if (!_productId.HasValue) return;

            _showLoading(true);
            try
            {
                await _model.UpdateProductAsync(_segment, _productId.Value, NameInput, DescriptionInput, BrandInput, PriceInput);
                GoBack!();
            }
            catch (Exception ex)
            {
                string msg = $"{StringProvider.GetString("ProductUploadError")}{ex.Message}";
                _showNotification(NotificationType.Error, msg);
            }
            finally { _showLoading(false); }
        }
    }
}
