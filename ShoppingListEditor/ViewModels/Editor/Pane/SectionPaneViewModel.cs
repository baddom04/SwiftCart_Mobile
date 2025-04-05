using DynamicData;
using ReactiveUI;
using ShoppingList.Shared;
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
    internal class SectionPaneViewModel : ViewModelBase
    {
        public ObservableCollection<SectionViewModel> Sections { get; }
        public bool IsEmptySections => Sections.Count == 0;
        public ReactiveCommand<Unit, Unit> GoBackCommand { get; }

        private readonly EditorModel _model;
        private readonly Action<bool> _showLoading;
        private readonly Action<NotificationType, string> _showNotification;
        private readonly Action _goBack;

        public SectionPaneViewModel(EditorModel model, Action<bool> showLoading, Action<NotificationType, string> showNotification, Action goBack)
        {
            _model = model;
            _showLoading = showLoading;
            _showNotification = showNotification;
            _goBack = goBack;
            Sections = [..GetSections()];
            GoBackCommand = ReactiveCommand.Create(_goBack);

            Sections.CollectionChanged += (s, e) => this.RaisePropertyChanged(nameof(IsEmptySections));
            _model.SectionsChanged += () =>
            {
                Sections.Clear();
                Sections.AddRange(GetSections());
            };
        }
        private IEnumerable<SectionViewModel> GetSections()
        {
            return _model.Store!.Map!.Sections.Select(s => new SectionViewModel(_model, s, _showLoading, _showNotification));
        }

        public async Task AddSection(string name)
        {
            _showLoading(true);
            try
            {
                await _model.AddSection(name);
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
