using DynamicData;
using ShoppingList.Core;
using ShoppingList.Model.Settings;
using ShoppingList.Model.ShoppingList;
using ShoppingList.Model.Social;
using ShoppingList.Utils;
using ShoppingList.ViewModels.Shared;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels.ShoppingList
{
    internal class HouseholdsGroceriesViewModel(UserAccountModel account, MyHouseholdsModel householdsModel, Action<ViewModelBase> changeToPage, Action<NotificationType, string> showNotification, Action<GroceryPage> changePage, Action<int, Grocery?, Action> changeToEditingPage) 
        : MyHouseholdsViewModel(account, householdsModel, changeToPage, showNotification)
    {
        private readonly Action<GroceryPage> _changePage = changePage;
        private readonly Action<int, Grocery?, Action> _changeToEditingPage = changeToEditingPage;

        public override async Task LoadMyHouseholds()
        {
            IsLoading = true;

            try
            {
                MyHouseholds.Clear();
                MyHouseholds.AddRange((await _model.GetMyHouseholds(_account.User!.Id)).Select(hh => new ShoppingListViewModel(new ShoppingListModel(hh.Id), hh, _changeToPage, _changePage, _showNotification, _changeToEditingPage)));
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
