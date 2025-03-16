using ReactiveUI;
using ShoppingList.Core;
using ShoppingList.Model.Settings;
using ShoppingList.Model.Social;
using ShoppingList.Utils;
using System;
using System.Collections.Generic;

namespace ShoppingList.ViewModels.Social
{
    internal class MainSocialPanelViewModel : DefaultPageOnChangeViewModel
    {
        private readonly Dictionary<SocialPage, ViewModelBase> _pages;

        private ViewModelBase _currentPage;
        public ViewModelBase CurrentPage
        {
            get { return _currentPage; }
            private set { this.RaiseAndSetIfChanged(ref _currentPage, value); }
        }

        private readonly CreateHouseholdViewModel _householdEditingPage;

        public MainSocialPanelViewModel(UserAccountModel account, Action<NotificationType, string> showNotification, Action<bool> showLoading)
        {
            _householdEditingPage = new CreateHouseholdViewModel(new CreateHouseholdModel(), ChangePage, showLoading);
            _pages = new Dictionary<SocialPage, ViewModelBase>()
            {
                { SocialPage.Main, new SocialPanelViewModel(new SocialPanelModel(), showNotification, ChangePage) },
                { SocialPage.ManageApplications, new ManageApplicationsViewModel(account, new ManageApplicationsModel(), showNotification, ChangePage) },
                { SocialPage.ManageHouseholds, new ManageHouseholdsViewModel(account, new ManageHouseholdsModel(), ChangePage, showNotification, ChangeToHouseholdPage, HouseholdEditingPage, showLoading) },
                { SocialPage.CreateHouseholdPage, _householdEditingPage },
            };

            _currentPage = _pages[SocialPage.Main];
        }

        private void ChangePage(SocialPage page)
        {
            CurrentPage = _pages[page];
        }
        private void ChangeToHouseholdPage(HouseholdViewModel householdViewModel)
        {
            CurrentPage = householdViewModel;
        }
        private void HouseholdEditingPage(Household? household)
        {
            _householdEditingPage.EditState(household);
            CurrentPage = _householdEditingPage;
        }

        public override void ChangeToDefaultPage()
        {
            CurrentPage = _pages[SocialPage.Main];
        }
    }

    internal enum SocialPage
    {
        Main,
        ManageApplications,
        ManageHouseholds,
        CreateHouseholdPage
    }
}
