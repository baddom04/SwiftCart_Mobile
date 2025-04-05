using DynamicData;
using ReactiveUI;
using ShoppingList.Shared.Utils;
using ShoppingListEditor.Model;
using ShoppingListEditor.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingListEditor.ViewModels.Editor.Pane
{
    internal class SectionPaneViewModel : PanePageViewModel
    {
        public ObservableCollection<SectionViewModel> Sections { get; }
        public bool IsEmptySections => Sections.Count == 0;
        public ReactiveCommand<Unit, Unit> GoBackCommand { get; }

        private readonly EditorModel _model;
        private readonly Action<bool> _showLoading;
        private readonly Action<NotificationType, string> _showNotification;
        public SectionPaneViewModel(EditorModel model, Action<bool> showLoading, Action<NotificationType, string> showNotification)
        {
            _model = model;
            _showLoading = showLoading;
            _showNotification = showNotification;
            Sections = [..GetSections()];
            GoBackCommand = ReactiveCommand.Create(() => GoBack!());

            Sections.CollectionChanged += (s, e) => this.RaisePropertyChanged(nameof(IsEmptySections));
            _model.SectionsChanged += () =>
            {
                Sections.Clear();
                Sections.AddRange(GetSections());
            };
        }
        private IEnumerable<SectionViewModel> GetSections()
        {
            if (_model.Store is null || _model.Store.Map is null) return [];
            return _model.Store.Map.Sections.Select(s => new SectionViewModel(_model, s, _showLoading, _showNotification));
        }

        public async Task AddSectionAsync(string name)
        {
            _showLoading(true);
            try
            {
                await _model.AddSectionAsync(name);
            }
            catch (Exception ex)
            {
                string msg = $"{StringProvider.GetString("SectionCreationError")}{ex.Message}";
                _showNotification(NotificationType.Error, msg);
            }
            finally { _showLoading(false); }
        }
    }
}
