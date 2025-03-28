using DynamicData;
using ReactiveUI;
using ShoppingList.Core;
using ShoppingList.Core.Enums;
using ShoppingList.Model.Social;
using ShoppingList.Shared.Model.Settings;
using ShoppingList.Shared.Utils;
using ShoppingList.Utils;
using ShoppingList.ViewModels.Shared;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels.Social
{
    internal class HouseholdViewModel : HouseholdListItemViewModel
    {
        public override ReactiveCommand<Unit, Unit> HouseholdOperationCommand { get; }
        public ReactiveCommand<Unit, Unit> GoBackCommand { get; }
        public ReactiveCommand<Unit, Unit> EditHouseholdCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteHouseholdCommand { get; }
        public ReactiveCommand<Unit, Unit> LeaveHouseholdCommand { get; }
        public ReactiveCommand<Unit, bool> MembersDropDownCommand { get; }
        public ReactiveCommand<Unit, bool> ApplicantsDropDownCommand { get; }

        private bool _isMemberLoading;
        public bool IsMemberLoading
        {
            get { return _isMemberLoading; }
            private set { this.RaiseAndSetIfChanged(ref _isMemberLoading, value); }
        }

        private bool _isApplicationLoading;
        public bool IsApplicationLoading
        {
            get { return _isApplicationLoading; }
            private set { this.RaiseAndSetIfChanged(ref _isApplicationLoading, value); }
        }

        private bool _isApplicationsOpen;
        public bool IsApplicationsOpen
        {
            get { return _isApplicationsOpen; }
            private set { this.RaiseAndSetIfChanged(ref _isApplicationsOpen, value); }
        }

        private bool _isMembersOpen;
        public bool IsMembersOpen
        {
            get { return _isMembersOpen; }
            private set { this.RaiseAndSetIfChanged(ref _isMembersOpen, value); }
        }
        public bool IsOwner { get; }

        public ObservableCollection<UserListItemViewModel> Members { get; } = [];
        public ObservableCollection<UserListItemViewModel> Applicants { get; } = [];

        private readonly Action<SocialPage> _changePage;
        private readonly Action<HouseholdViewModel> _changeToHouseholdPage;
        private readonly HouseholdModel _model;
        private readonly Action<NotificationType, string> _showNotification;
        private readonly Action<bool> _showLoading;
        private readonly UserAccountModel _account;
        public HouseholdViewModel(UserAccountModel account, HouseholdModel model, Action<SocialPage> changePage, Action<HouseholdViewModel> changeToHouseholdPage, Action<Household?> householdEditingPage, Action<NotificationType, string> showNotification, Action<bool> showLoading)
        {
            _name = model.Household.Name;
			_identifier = model.Household.Identifier;
            _changePage = changePage;
            _showNotification = showNotification;
            _showLoading = showLoading;
            _changeToHouseholdPage = changeToHouseholdPage;
            _account = account;
            _model = model;
            IsOwner = model.Household.Relationship == HouseholdRelationship.Owner;


            DeleteHouseholdCommand = ReactiveCommand.CreateFromTask(DeleteHouseholdAsync);
            LeaveHouseholdCommand = ReactiveCommand.CreateFromTask(LeaveHouseholdAsync);
            HouseholdOperationCommand = ReactiveCommand.Create(() => _changeToHouseholdPage(this));
            GoBackCommand = ReactiveCommand.Create(() => _changePage(SocialPage.ManageHouseholds));
            EditHouseholdCommand = ReactiveCommand.Create(() => householdEditingPage(_model.Household));
            MembersDropDownCommand = ReactiveCommand.Create(() => IsMembersOpen = !IsMembersOpen);
            ApplicantsDropDownCommand = ReactiveCommand.Create(() => IsApplicationsOpen = !IsApplicationsOpen);

            this.WhenAnyValue(x => x.IsMembersOpen).Subscribe(async b => { if (b) await LoadMembersAsync(); });
            this.WhenAnyValue(x => x.IsApplicationsOpen).Subscribe(async b => { if (b) await LoadApplicantsAsync(); });
        }
        private async Task LeaveHouseholdAsync()
        {
            _showLoading(true);

            try
            {
                await _model.LeaveHousholdAsync(_account.User!.Id);
                _changePage(SocialPage.ManageHouseholds);
            }
            catch (Exception ex)
            {
                string msg = $"{StringProvider.GetString("DeleteHouseholdError")}{ex.Message}";
                _showNotification(NotificationType.Error, msg);
            }
            finally
            {
                _showLoading(false);
            }
        }
        private async Task DeleteHouseholdAsync()
        {
            _showLoading(true);

            try
            {
                await _model.DeleteHouseholdAsync();
                _changePage(SocialPage.ManageHouseholds);
            }
            catch (Exception ex)
            {
                string msg = $"{StringProvider.GetString("DeleteHouseholdError")}{ex.Message}";
                _showNotification(NotificationType.Error, msg);
            }
            finally
            {
                _showLoading(false);
            }
        }

        private async Task LoadMembersAsync()
        {
            IsMemberLoading = true;

            try
            {
                Members.Clear();
                Members.AddRange(
                    (await _model.GetHouseholdMembersAsync())
                    .Select(u => new UserListItemViewModel(IsOwner, _account, _model.Household.Id, new UserListItemModel(u), _showNotification))
                );
            }
            catch (Exception ex)
            {
                string msg = $"{StringProvider.GetString("LoadMembersError")}{ex.Message}";
                _showNotification(NotificationType.Error, msg);
            }
            finally
            {
                IsMemberLoading = false;
            }
        }

        private async Task LoadApplicantsAsync()
        {
            IsApplicationLoading = true;

            try
            {
                Applicants.Clear();
                Applicants.AddRange(
                    (await _model.GetApplicantsAsync())
                    .Select(u => new UserListItemViewModel(IsOwner, _account, _model.Household.Id, new UserListItemModel(u), _showNotification))
                );
            }
            catch (Exception ex)
            {
                string msg = $"{StringProvider.GetString("LoadMembersError")}{ex.Message}";
                _showNotification(NotificationType.Error, msg);
            }
            finally
            {
                IsApplicationLoading = false;
            }
        }
    }
}
