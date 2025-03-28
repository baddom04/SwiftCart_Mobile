using ReactiveUI;
using ShoppingList.Core;
using ShoppingList.Shared;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;

namespace ShoppingList.ViewModels.Map
{
    internal class SectionViewModel : ViewModelBase
    {
        public ObservableCollection<ProductViewModel> Products { get; }

        private bool _isOpen;
        public bool IsOpen
        {
            get { return _isOpen; }
            private set { this.RaiseAndSetIfChanged(ref _isOpen, value); }
        }
        public ReactiveCommand<Unit, bool> OpenCommand { get; }

        public Section Section { get; }
        public SectionViewModel(Section section, IEnumerable<ProductViewModel> products)
        {
            Section = section;

            Products = [.. products];
            OpenCommand = ReactiveCommand.Create(() => IsOpen = !IsOpen);
        }
    }
}
