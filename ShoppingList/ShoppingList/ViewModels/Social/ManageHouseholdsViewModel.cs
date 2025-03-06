using DynamicData;
using ReactiveUI;
using ShoppingList.Core;
using ShoppingList.Model.Settings;
using ShoppingList.Model.Social;
using ShoppingList.Utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels.Social
{
    internal class ManageHouseholdsViewModel : ViewModelBase
    {
        public ObservableCollection<HouseholdViewModel> MyHouseholds { get; }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { this.RaiseAndSetIfChanged(ref _isLoading, value); }
        }

        public bool EmptyMyHouseholds => MyHouseholds.Count == 0;
        public ReactiveCommand<Unit, Unit> GoBackCommand { get; }
        public ReactiveCommand<Unit, Unit> CreateHouseholdPageCommand { get; }

        private readonly Action<SocialPage> _changePage;
        private readonly Action<NotificationType, string> _showNotification;
        private readonly Action<HouseholdViewModel> _changeToHouseholdPage;
        private readonly Action<Household?> _householdEditingPage;
        private readonly ManageHouseholdsModel _model;
        private readonly UserAccountModel _account;
        public ManageHouseholdsViewModel(UserAccountModel account, ManageHouseholdsModel model, Action<SocialPage> changePage, Action<NotificationType, string> showNotification, Action<HouseholdViewModel> changeToHouseholdPage, Action<Household?> householdEditingPage)
        {
            _changePage = changePage;
            _account = account;
            _model = model;
            _showNotification = showNotification;
            _changeToHouseholdPage = changeToHouseholdPage;
            _householdEditingPage = householdEditingPage;
            GoBackCommand = ReactiveCommand.Create(() => _changePage(SocialPage.Main));
            CreateHouseholdPageCommand = ReactiveCommand.Create(() => _householdEditingPage(null));

            MyHouseholds = [];
            MyHouseholds.CollectionChanged += (s, e) => this.RaisePropertyChanged(nameof(EmptyMyHouseholds));
        }

        public async Task LoadMyHouseholds()
        {
            IsLoading = true;

            try
            {
                MyHouseholds.Clear();
                MyHouseholds.AddRange((await _model.GetMyHouseholds(_account.User!.Id)).Select(hh => new HouseholdViewModel(hh, _changePage, _changeToHouseholdPage, _householdEditingPage)));
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
