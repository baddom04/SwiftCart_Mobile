using ShoppingList.Core;
using ShoppingList.Model.Map;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ShoppingList.ViewModels.Map
{
    internal class SectionViewModel : ViewModelBase
    {
        public ObservableCollection<ProductViewModel> Products { get; }
        public bool IsOpen { get; set; }

        private readonly MapModel _model;
        public Section Section { get; }
        public SectionViewModel(MapModel model, Section section, IEnumerable<ProductViewModel> products)
        {
            _model = model;
            Section = section;

            Products = [.. products];
        }
    }
}
