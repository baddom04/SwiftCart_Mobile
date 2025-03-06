using ReactiveUI;
using ShoppingList.Model.Settings;
using ShoppingList.Model.Social;
using ShoppingList.Utils;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels.Social
{
    internal class HouseholdApplicationViewModel : ViewModelBase
    {
        private readonly int _householdId;

        private string _name;
        public string Name
        {
            get { return _name; }
            private set { this.RaiseAndSetIfChanged(ref _name, value); }
        }

        private string _identifier;
        public string Identifier
        {
            get { return _identifier; }
            private set { this.RaiseAndSetIfChanged(ref _identifier, value); }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { this.RaiseAndSetIfChanged(ref _isLoading, value); }
        }

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

        public HouseholdApplicationViewModel(UserAccountModel account, HouseholdApplicationModel model, Action<NotificationType, string> showNotification)
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