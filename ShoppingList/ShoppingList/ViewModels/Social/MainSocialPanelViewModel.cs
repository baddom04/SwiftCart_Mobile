using ReactiveUI;
using ShoppingList.Model.Settings;
using ShoppingList.Model.Social;
using ShoppingList.Utils;
using System;
using System.Collections.Generic;

namespace ShoppingList.ViewModels.Social
{
    internal class MainSocialPanelViewModel : ViewModelBase
    {
        private readonly Dictionary<SocialPage, ViewModelBase> _pages;

        private ViewModelBase _currentPage;
        public ViewModelBase CurrentPage
        {
            get { return _currentPage; }
            private set { this.RaiseAndSetIfChanged(ref _currentPage, value); }
        }

        public MainSocialPanelViewModel(UserAccountModel account, Action<NotificationType, string> showNotification, Action<bool> showLoading)
        {
            _pages = new Dictionary<SocialPage, ViewModelBase>()
            {
                { SocialPage.Main, new SocialPanelViewModel(new SocialPanelModel(), showNotification, ChangePage) },
                { SocialPage.ManageApplications, new ManageApplicationsViewModel(account, new ManageApplicationsModel(), showNotification, ChangePage) },
                { SocialPage.ManageHouseholds, new ManageHouseholdsViewModel(account, new ManageHouseholdsModel(), ChangePage, showNotification, ChangeToHouseholdPage) },
                { SocialPage.CreateHouseholdPage, new CreateHouseholdViewModel(new CreateHouseholdModel(), ChangePage, showLoading) },
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
    }

    internal enum SocialPage
    {
        Main,
        ManageApplications,
        ManageHouseholds,
        CreateHouseholdPage
    }
}
