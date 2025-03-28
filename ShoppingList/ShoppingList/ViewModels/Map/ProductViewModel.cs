using ReactiveUI;
using ShoppingList.Core;
using ShoppingList.Model.Map;
using ShoppingList.Shared;
using System.Reactive;

namespace ShoppingList.ViewModels.Map
{
    internal class ProductViewModel : ViewModelBase
    {
        private bool _isOpen;
        public bool IsOpen
        {
            get { return _isOpen; }
            private set { this.RaiseAndSetIfChanged(ref _isOpen, value); }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { this.RaiseAndSetIfChanged(ref _isSelected, value); OnSelectedChanged(); }
        }

        public Product Product { get; }
        public ReactiveCommand<Unit, bool> OpenCommand { get; }

        private readonly MapModel _model;

        public ProductViewModel(MapModel model, Product product)
        {
            _model = model;
            Product = product;
            OpenCommand = ReactiveCommand.Create(() => IsOpen = !IsOpen);
        }
        private void OnSelectedChanged()
        {
            if (_isSelected)
                _model.Select(Product);
            else
                _model.UnSelect(Product);
        }
    }
}
