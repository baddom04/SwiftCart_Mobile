using ReactiveUI;
using ShoppingList.Core.Enums;
using ShoppingList.Shared;
using ShoppingList.Shared.Utils;
using ShoppingListEditor.Model;
using ShoppingListEditor.Model.Editables;
using ShoppingListEditor.Utils;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingListEditor.ViewModels.Editor
{
    internal class MapSegmentViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> OpenDetailPaneCommand { get; }
        public ReactiveCommand<Unit, Unit> CloseDetailPaneCommand { get; }
        public ReactiveCommand<SegmentType, Unit> UploadSegmentCommand { get; }

        private IReadOnlyCollection<SectionEditable> _sections;
        public IReadOnlyCollection<SectionEditable> Sections
        {
            get { return _sections; }
            private set { this.RaiseAndSetIfChanged(ref _sections, value); }
        }

        private SectionEditable? _selectedSection;
        public SectionEditable? SelectedSection
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
        public MapSegmentViewModel(MapSegmentEditable segment, EditorModel model, Action<ViewModelBase?> setPaneContent, Action<bool> showLoading, Action<NotificationType, string> showNotification)
        {
            _segment = segment;
            _showLoading = showLoading;
            _showNotification = showNotification;

            X = segment.X;
            Y = segment.Y;
            Type = segment.Type;

            _segment.TypeChanged += () => Type = segment.Type;

            _model = model;
            _sections = [.. _model.Store!.Map!.Sections];

            OpenDetailPaneCommand = ReactiveCommand.Create(() => setPaneContent(this));
            CloseDetailPaneCommand = ReactiveCommand.Create(() => setPaneContent(null));
            UploadSegmentCommand = ReactiveCommand.CreateFromTask<SegmentType>(UploadSegmentAsync);
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
