using ReactiveUI;
using ShoppingList.Model.Social;
using ShoppingList.Shared.Model.Settings;
using ShoppingList.Shared.Utils;
using ShoppingList.Utils;
using ShoppingList.ViewModels.Shared;
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

        public override ReactiveCommand<Unit, Unit> HouseholdOperationCommand { get; }

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

            HouseholdOperationCommand = ReactiveCommand.CreateFromTask(DeleteApplicationAsync);
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