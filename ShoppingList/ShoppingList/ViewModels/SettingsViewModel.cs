using ReactiveUI;
using ShoppingList.Model.Models;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels
{
    internal class SettingsViewModel : ViewModelBase
    {
        private readonly UserAccountModel _userAccount;
        public ReactiveCommand<Unit, Unit> LogoutCommand { get; }

        private readonly Action<bool> _showLoading;
        private readonly Action<Page> _changePage;

        public SettingsViewModel(UserAccountModel userAccount, Action<Page> changePage, Action<bool> showLoading)
        {
            _userAccount = userAccount;
            _changePage = changePage;
            _showLoading = showLoading;
            LogoutCommand = ReactiveCommand.CreateFromTask(Logout);
        }

        private async Task Logout()
        {
            _showLoading(true);

            try
            {
                await _userAccount.LogoutAsync();

                _changePage(Page.Login);
            }
            catch (Exception ex)
            {
                //TODO: create a universal 'dialog' window, and show an error to the user
                throw new NotImplementedException();
            }
            finally
            {
                _showLoading(false);
            }
        }
    }
}
