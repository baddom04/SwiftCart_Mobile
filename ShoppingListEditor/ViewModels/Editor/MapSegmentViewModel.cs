using ReactiveUI;
using ShoppingList.Shared;
using ShoppingListEditor.Model;
using ShoppingListEditor.Model.Editables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;

namespace ShoppingListEditor.ViewModels.Editor
{
    internal class MapSegmentViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> RemoveSegmentDataCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenDetailPaneCommand { get; }
        public ReactiveCommand<Unit, Unit> CloseDetailPaneCommand { get; }

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


        private readonly EditorModel _model;
        public MapSegmentViewModel(MapSegmentEditable segment, EditorModel model, Action<ViewModelBase?> setPaneContent)
        {
            _model = model;
            _sections = [.. _model.Store!.Map!.Sections];

            OpenDetailPaneCommand = ReactiveCommand.Create(() => setPaneContent(this));
            RemoveSegmentDataCommand = ReactiveCommand.Create(() => _model.SetSegmentEmpty(segment));
            CloseDetailPaneCommand = ReactiveCommand.Create(() => setPaneContent(null));
        }
    }
}
