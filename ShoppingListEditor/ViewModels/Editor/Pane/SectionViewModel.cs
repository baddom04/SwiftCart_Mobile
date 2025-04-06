using ReactiveUI;
using ShoppingList.Shared;
using ShoppingList.Shared.Utils;
using ShoppingListEditor.Model;
using ShoppingListEditor.Model.Editables;
using ShoppingListEditor.Utils;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingListEditor.ViewModels.Editor.Pane
{
    internal class SectionViewModel : ViewModelBase
    {
        private readonly EditorModel _model;
        private readonly SectionEditable _section;
        private readonly Action<bool> _showLoading;
        private readonly Action<NotificationType, string> _showNotification;
        public string Name { get; set; }
        public ReactiveCommand<Unit, Unit> DeleteSectionCommand { get; }
        public SectionViewModel(EditorModel model, SectionEditable section, Action<bool> showLoading, Action<NotificationType, string> showNotification)
        {
            _model = model;
            _section = section;
            _showLoading = showLoading;
            _showNotification = showNotification;

            Name = section.Name;
            DeleteSectionCommand = ReactiveCommand.CreateFromTask(DeleteSectionAsnyc);
        }
        public async Task UpdateSectionAsync(string name)
        {
            string oldName = _section.Name;
            _showLoading(true);
            try
            {
                _section.Name = name.Trim();
                await _model.UpdateSectionAsync(_section);
            }
            catch (Exception ex)
            {
                string msg = $"{StringProvider.GetString("SectionUpdateError")}{ex.Message}";
                _showNotification(NotificationType.Error, msg);
                _section.Name = oldName;
            }
            finally 
            { 
                _showLoading(false); 
            }
        }
        private async Task DeleteSectionAsnyc()
        {
            _showLoading(true);
            try
            {
                await _model.RemoveSectionAsync(_section);
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

