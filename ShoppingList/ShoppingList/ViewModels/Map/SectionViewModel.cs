using ShoppingList.Core;
using ShoppingList.Model.Map;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace ShoppingList.ViewModels.Map
{
    internal class SectionViewModel : ViewModelBase
    {
        public ObservableCollection<ProductViewModel> Products { get; }
        public ObservableCollection<ProductViewModel> SelectedProducts { get; } = [];
        public bool IsOpen { get; set; }
        private readonly MapModel _model;
        public Section Section { get; }
        public SectionViewModel(MapModel model, Section section, IEnumerable<Product> products)
        {
            _model = model;
            Section = section;

            Products = [.. products.Select(product => new ProductViewModel(product))];

            SelectedProducts.CollectionChanged += SelectedProducts_CollectionChanged;
        }

        private void SelectedProducts_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems is null) break;
                    foreach (ProductViewModel newItem in e.NewItems)
                    {
                        _model.Select(newItem.Product);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems is null) break;
                    foreach (ProductViewModel oldItem in e.OldItems)
                    {
                        _model.UnSelect(oldItem.Product);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
