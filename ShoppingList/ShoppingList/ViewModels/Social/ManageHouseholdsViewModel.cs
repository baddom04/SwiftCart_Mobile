using DynamicData;
using ReactiveUI;
using ShoppingList.Core;
using ShoppingList.Model.Social;
using ShoppingList.Shared;
using ShoppingList.Shared.Model.Settings;
using ShoppingList.Shared.Utils;
using ShoppingList.Utils;
using ShoppingList.ViewModels.Shared;
using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels.Social
{
    internal class ManageHouseholdsViewModel : MyHouseholdsViewModel
    {
        public ReactiveCommand<Unit, Unit> GoBackCommand { get; }
        public ReactiveCommand<Unit, Unit> CreateHouseholdPageCommand { get; }

        private readonly Action<SocialPage> _changePage;
        private readonly Action<Household?> _householdEditingPage;
        private readonly Action<bool> _showLoading;
        public ManageHouseholdsViewModel(UserAccountModel account, MyHouseholdsModel model, Action<SocialPage> changePage, Action<NotificationType, string> showNotification, Action<ViewModelBase> changeToHouseholdPage, Action<Household?> householdEditingPage, Action<bool> showLoading) : base(account, model, changeToHouseholdPage, showNotification)
        {
            _changePage = changePage;
            _showLoading = showLoading;
            _householdEditingPage = householdEditingPage;
            GoBackCommand = ReactiveCommand.Create(() => _changePage(SocialPage.Main));
            CreateHouseholdPageCommand = ReactiveCommand.Create(() => _householdEditingPage(null));
        }

        public override async Task LoadMyHouseholds()
        {
            IsLoading = true;

            try
            {
                MyHouseholds.Clear();
                MyHouseholds.AddRange((await _model.GetMyHouseholds(_account.User!.Id)).Select(hh => new HouseholdViewModel(_account, new HouseholdModel(hh), _changePage, _changeToPage, _householdEditingPage, _showNotification, _showLoading)));
            }
            catch (Exception ex)
            {
                string message = $"{StringProvider.GetString("MyHouseholdsQueryError")}{ex.Message}";
                _showNotification(NotificationType.Error, message);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
