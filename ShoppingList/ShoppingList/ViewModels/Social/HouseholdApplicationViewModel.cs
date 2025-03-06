using ReactiveUI;
using ShoppingList.Model.Settings;
using ShoppingList.Model.Social;
using ShoppingList.Utils;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels.Social
{
    internal class HouseholdApplicationViewModel : HouseholdListItemViewModel
    {
        private readonly int _householdId;

        private bool _deleted;
        public bool Deleted
        {
            get { return _deleted; }
            set { this.RaiseAndSetIfChanged(ref _deleted, value); }
        }

        public ReactiveCommand<Unit, Unit> DeleteCommand { get; }

        private readonly Action<NotificationType, string> _showNotification;
        private readonly HouseholdApplicationModel _model;
        private readonly UserAccountModel _account;

        public HouseholdApplicationViewModel(UserAccountModel account, HouseholdApplicationModel model, Action<NotificationType, string> showNotification) : base()
        {
            _model = model;
            _account = account;
            _showNotification = showNotification;

            _name = _model.Household.Name;
            _identifier = _model.Household.Identifier;
            _householdId = _model.Household.Id;

            DeleteCommand = ReactiveCommand.CreateFromTask(DeleteApplicationAsync);
        }

        private async Task DeleteApplicationAsync()
        {
            IsLoading = true;

            try
            {
                await _model.DeleteApplicationAsync(_householdId, _account.User!.Id);
                Deleted = true;
            }
            catch (Exception ex)
            {
                string message = $"{StringProvider.GetString("DeleteApplicationError")}{ex.Message}";
                _showNotification(NotificationType.Error, message);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}