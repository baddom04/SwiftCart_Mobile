using DynamicData;
using ReactiveUI;
using ShoppingList.Core.Enums;
using ShoppingList.Shared;
using ShoppingList.Shared.Utils;
using ShoppingListEditor.Model;
using ShoppingListEditor.Model.Editables;
using ShoppingListEditor.Utils;
using ShoppingListEditor.ViewModels.Editor.Pane;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingListEditor.ViewModels.Editor
{
    internal class MapSegmentViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> OpenDetailPaneCommand { get; }
        public Action OpenDetailPane { get; }
        public ReactiveCommand<Unit, Unit> CloseDetailPaneCommand { get; }
        public ReactiveCommand<Unit, Unit> ToSectionPageCommand { get; }
        public ReactiveCommand<Unit, Unit> ToProductPageCommand { get; }
        public ReactiveCommand<SegmentType, Unit> UploadSegmentCommand { get; }
        public ReactiveCommand<Unit, Unit> PasteProductCommand { get; }
        public ObservableCollection<SectionEditable> Sections { get; }
        public ObservableCollection<ProductViewModel> Products { get; }
        public bool IsProductsEmpty => Products.Count == 0;

        private int _selectedSectionIndex;
        public int SelectedSectionIndex
        {
            get { return _selectedSectionIndex; }
            set { this.RaiseAndSetIfChanged(ref _selectedSectionIndex, value); }
        }
        public int X { get; }
        public int Y { get; }

        private SegmentType _type;
        public SegmentType Type
        {
            get { return _type; }
            private set { this.RaiseAndSetIfChanged(ref _type, value); }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { this.RaiseAndSetIfChanged(ref _isSelected, value); }
        }


        private readonly EditorModel _model;
        private readonly MapSegmentEditable _segment;
        private readonly Action<bool> _showLoading;
        private readonly Action<NotificationType, string> _showNotification;
        private readonly Action<ViewModelBase?> _setPaneContent;
        private int _previousSectionIndex;
        private readonly Action<ProductEditable> _setProductClipBoard;
        public MapSegmentViewModel(MapSegmentEditable segment, EditorModel model, Action<ViewModelBase?> setPaneContent, Action<bool> showLoading, Action<NotificationType, string> showNotification, Action<ProductEditable> setProductClipBoard, Func<ProductEditable?> getProductOnClipBoard)
        {
            _segment = segment;
            _model = model;
            _setPaneContent = setPaneContent;
            _showLoading = showLoading;
            _showNotification = showNotification;
            _setProductClipBoard = setProductClipBoard;

            X = segment.X;
            Y = segment.Y;
            Type = segment.Type;

            Sections = [
                new SectionEditable()
                {
                    Id = -1,
                    Name = StringProvider.GetString("None"),
                    MapId = _segment.MapId,
                },
            ];
            Products = [.. GetProducts()];
            _segment.Products.CollectionChanged += (s, e) =>
            {
                Products.Clear();
                Products.AddRange(GetProducts());
            };


            if (_model.Store is not null && _model.Store.Map is not null)
                Sections.AddRange(_model.Store.Map.Sections);


            _selectedSectionIndex = Sections.IndexOf(Sections.FirstOrDefault(s => s.Id == _segment.SectionId) ?? Sections[0]);
            _previousSectionIndex = _selectedSectionIndex;
            Products.CollectionChanged += (s, e) => this.RaisePropertyChanged(nameof(IsProductsEmpty));
            _segment.TypeChanged += () => Type = segment.Type;

            _model.SectionsChanged += () =>
            {
                Sections.Clear();
                Sections.Add(new SectionEditable()
                {
                    Id = -1,
                    Name = StringProvider.GetString("None"),
                    MapId = _segment.MapId,
                });

                if (_model.Store is not null && _model.Store.Map is not null)
                    Sections.AddRange(_model.Store!.Map!.Sections);

                _selectedSectionIndex = Sections.IndexOf(Sections.FirstOrDefault(s => s.Id == _segment.SectionId) ?? Sections[0]);
            };

            OpenDetailPane = () => setPaneContent(this);
            OpenDetailPaneCommand = ReactiveCommand.Create(OpenDetailPane);
            CloseDetailPaneCommand = ReactiveCommand.Create(() => setPaneContent(null));
            UploadSegmentCommand = ReactiveCommand.CreateFromTask<SegmentType>(UploadSegmentAsync);
            ToSectionPageCommand = ReactiveCommand.Create(() => setPaneContent(new SectionPaneViewModel(_model, _showLoading, _showNotification) { GoBack = () => setPaneContent(this) }));
            ToProductPageCommand = ReactiveCommand.Create(() => setPaneContent(new ProductPaneViewModel(_model, _segment, _showLoading, _showNotification) { GoBack = () => setPaneContent(this) }));
            PasteProductCommand = ReactiveCommand.CreateFromTask(async () => await CreateProductAsync(getProductOnClipBoard()));

            this.WhenAnyValue(x => x.SelectedSectionIndex).Subscribe(async (section) => await OnSelectedSectionChangedAsync(section));
        }
        private IEnumerable<ProductViewModel> GetProducts()
        {
            return [.. _segment.Products.Select(p => new ProductViewModel(_model, _segment, p, _showLoading, _showNotification, _setPaneContent, () => _setPaneContent(this), _setProductClipBoard))];
        }

        private bool _first = true;
        private async Task OnSelectedSectionChangedAsync(int index)
        {
            if (_first)
            {
                _first = false;
                return;
            }

            if (index == -1 || index == _previousSectionIndex) return;

            _showLoading(true);
            try
            {
                await _model.ChangeSectionOnSegmentAsync(_segment, Sections[index]);
                _previousSectionIndex = index;
            }
            catch (Exception ex)
            {
                string msg = $"{StringProvider.GetString("SectionChangeError")}{ex.Message}";
                _showNotification(NotificationType.Error, msg);
                SelectedSectionIndex = _previousSectionIndex;
            }
            finally
            {
                _showLoading(false);
            }
        }
        private async Task UploadSegmentAsync(SegmentType type)
        {
            _showLoading(true);
            try
            {
                if (type == SegmentType.Empty)
                    await _model.DeleteSegmentAsync(_segment);
                else
                    await _model.UploadSegmentAsync(_segment, type);
                _setPaneContent(null);
            }
            catch (Exception ex)
            {
                string msg = $"{StringProvider.GetString("SegmentUploadError")}{ex.Message}";
                _showNotification(NotificationType.Error, msg);
            }
            finally
            {
                _showLoading(false);
            }
        }
        private async Task CreateProductAsync(ProductEditable? product)
        {
            if (product == null) return;
            _showLoading(true);
            try
            {
                await _model.CreateProductAsync(_segment, product.Name, product.Description, product.Brand, product.Price.ToString());
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
