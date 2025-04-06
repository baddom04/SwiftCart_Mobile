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
        public ReactiveCommand<Unit, Unit> CloseDetailPaneCommand { get; }
        public ReactiveCommand<Unit, Unit> ToSectionPageCommand { get; }
        public ReactiveCommand<Unit, Unit> ToProductPageCommand { get; }
        public ReactiveCommand<SegmentType, Unit> UploadSegmentCommand { get; }
        public ObservableCollection<SectionEditable> Sections { get; }
        public ObservableCollection<ProductViewModel> Products { get; }
        public bool IsProductsEmpty => Products.Count == 0;

        private int _selectedSection;
        public int SelectedSection
        {
            get { return _selectedSection; }
            set { this.RaiseAndSetIfChanged(ref _selectedSection, value); }
        }
        public int X { get; }
        public int Y { get; }

        private SegmentType _type;
        public SegmentType Type
        {
            get { return _type; }
            private set { this.RaiseAndSetIfChanged(ref _type, value); }
        }

        private readonly EditorModel _model;
        private readonly MapSegmentEditable _segment;
        private readonly Action<bool> _showLoading;
        private readonly Action<NotificationType, string> _showNotification;
        private readonly Action<ViewModelBase?> _setPaneContent;
        public MapSegmentViewModel(MapSegmentEditable segment, EditorModel model, Action<ViewModelBase?> setPaneContent, Action<bool> showLoading, Action<NotificationType, string> showNotification)
        {
            _segment = segment;
            _model = model;
            _setPaneContent = setPaneContent;
            _showLoading = showLoading;
            _showNotification = showNotification;

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


            _selectedSection = Sections.IndexOf(Sections.FirstOrDefault(s => s.Id == _segment.SectionId) ?? Sections[0]);
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

                _selectedSection = Sections.IndexOf(Sections.FirstOrDefault(s => s.Id == _segment.SectionId) ?? Sections[0]);
            };

            OpenDetailPaneCommand = ReactiveCommand.Create(() => setPaneContent(this));
            CloseDetailPaneCommand = ReactiveCommand.Create(() => setPaneContent(null));
            UploadSegmentCommand = ReactiveCommand.CreateFromTask<SegmentType>(UploadSegmentAsync);
            ToSectionPageCommand = ReactiveCommand.Create(() => setPaneContent(new SectionPaneViewModel(_model, _showLoading, _showNotification) { GoBack = () => setPaneContent(this) }));
            ToProductPageCommand = ReactiveCommand.Create(() => setPaneContent(new ProductPaneViewModel(_model, _segment, _showLoading, _showNotification) { GoBack = () => setPaneContent(this) }));

            this.WhenAnyValue(x => x.SelectedSection).Subscribe(async (section) => await OnSelectedSectionChangedAsync(section));
        }
        private IEnumerable<ProductViewModel> GetProducts()
        {
            return [.. _segment.Products.Select(p => new ProductViewModel(_model, _segment, p, _showLoading, _showNotification, _setPaneContent, () => _setPaneContent(this)))];
        }

        private bool _first = true;
        private async Task OnSelectedSectionChangedAsync(int index)
        {
            if (_first)
            {
                _first = false;
                return;
            }

            if (index == -1) return;

            _showLoading(true);
            try
            {
                await _model.ChangeSectionOnSegmentAsync(_segment, Sections[index]);
            }
            catch (Exception ex)
            {
                string msg = $"{StringProvider.GetString("SectionChangeError")}{ex.Message}";
                _showNotification(NotificationType.Error, msg);
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
                _segment.Type = type;
                if (type == SegmentType.Empty)
                    await _model.DeleteSegmentAsync(_segment);
                else
                    await _model.UploadSegmentAsync(_segment);
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
    }
}
