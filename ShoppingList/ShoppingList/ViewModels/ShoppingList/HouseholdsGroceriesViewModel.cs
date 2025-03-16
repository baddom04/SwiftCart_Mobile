using DynamicData;
using ShoppingList.Model.Settings;
using ShoppingList.Model.Social;
using ShoppingList.Utils;
using ShoppingList.ViewModels.Shared;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels.ShoppingList
{
    internal class HouseholdsGroceriesViewModel : MyHouseholdsViewModel
    {
        private readonly MyHouseholdsModel _householdsModel;
        public HouseholdsGroceriesViewModel(UserAccountModel account, MyHouseholdsModel householdsModel, Action<ViewModelBase> changeToPage, Action<NotificationType, string> showNotification) : base(account, householdsModel, changeToPage, showNotification)
        {
            _householdsModel = householdsModel;
        }
        public override async Task LoadMyHouseholds()
        {
            IsLoading = true;

            try
            {
                MyHouseholds.Clear();
                //MyHouseholds.AddRange((await _model.GetMyHouseholds(_account.User!.Id)).Select(hh => new HouseholdViewModel(_account, new HouseholdModel(hh), _changePage, _changeToPage, _householdEditingPage, _showNotification, _showLoading)));
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
