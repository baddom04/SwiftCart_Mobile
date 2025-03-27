using ReactiveUI;
using ShoppingList.Core;
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

        public Product Product { get; }
        public ReactiveCommand<Unit, bool> OpenCommand { get; }

        public ProductViewModel(Product product)
        {
            Product = product;
            OpenCommand = ReactiveCommand.Create(() => IsOpen = !IsOpen);
        }
    }
}
