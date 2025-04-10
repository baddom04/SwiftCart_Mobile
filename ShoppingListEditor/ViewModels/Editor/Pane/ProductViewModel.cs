﻿using ReactiveUI;
using ShoppingList.Shared;
using ShoppingList.Shared.Utils;
using ShoppingListEditor.Model;
using ShoppingListEditor.Model.Editables;
using ShoppingListEditor.Utils;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingListEditor.ViewModels.Editor.Pane
{
    internal class ProductViewModel : ViewModelBase
    {
        private bool _isOpen;
        public bool IsOpen
        {
            get { return _isOpen; }
            private set { this.RaiseAndSetIfChanged(ref _isOpen, value); }
        }

        public string Name { get; }
        public string Brand { get; }
        public string Description { get; }
        public decimal Price { get; }
        public ReactiveCommand<Unit, Unit> UpdateCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteCommand { get; }
        public ReactiveCommand<Unit, bool> OpenCommand { get; }
        public ReactiveCommand<Unit, Unit> ToClipBoardCommand { get; }

        private readonly EditorModel _model;
        private readonly ProductEditable _product;
        private readonly MapSegmentEditable _segment;
        private readonly Action<bool> _showLoading;
        private readonly Action<NotificationType, string> _showNotification;
        public ProductViewModel(EditorModel model, MapSegmentEditable segment, ProductEditable product, Action<bool> showLoading, Action<NotificationType, string> showNotification, Action<ViewModelBase?> setPaneContent, Action goBack, Action<ProductEditable> setProductClipBoard)
        {
            _model = model;
            _segment = segment;
            _product = product;
            _showLoading = showLoading;
            _showNotification = showNotification;

            Name = product.Name;
            Brand = product.Brand;
            Description = product.Description ?? string.Empty;
            Price = product.Price;

            UpdateCommand = ReactiveCommand.Create(() => setPaneContent(new ProductPaneViewModel(_model, _segment, _product, _showLoading, _showNotification) { GoBack = goBack }));
            DeleteCommand = ReactiveCommand.CreateFromTask(DeleteProductAsync);
            OpenCommand = ReactiveCommand.Create(() => IsOpen = !IsOpen);
            ToClipBoardCommand = ReactiveCommand.Create(() => setProductClipBoard(_product));
        }

        private async Task DeleteProductAsync()
        {
            _showLoading(true);
            try
            {
                await _model.DeleteProductAsync(_segment, _product);
            }
            catch (Exception ex)
            {
                string msg = $"{StringProvider.GetString("ProductDeleteError")}{ex.Message}";
                _showNotification(NotificationType.Error, msg);
            }
            finally
            {
                _showLoading(false);
            }
        }
    }
}
