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

        public MainSocialPanelViewModel(UserAccountModel account, ManageApplicationsModel manageApplications, MainSocialPanelModel households, ManageHouseholdsModel manageHouseholds, CreateHouseholdModel createHousehold, Action<NotificationType, string> showNotification, Action<bool> showLoading)
        {
            _pages = new Dictionary<SocialPage, ViewModelBase>()
            {
                { SocialPage.Main, new SocialPanelViewModel(households, showNotification, ChangePage) },
                { SocialPage.ManageApplications, new ManageApplicationsViewModel(account, manageApplications, showLoading, showNotification, ChangePage) },
                { SocialPage.ManageHouseholds, new ManageHouseholdsViewModel(account, manageHouseholds, ChangePage, showNotification) },
                { SocialPage.CreateHouseholdPage, new CreateHouseholdViewModel(createHousehold, ChangePage) },
            };

            _currentPage = _pages[SocialPage.Main];
        }

        private void ChangePage(SocialPage page)
        {
            CurrentPage = _pages[page];
        }
    }

    internal enum SocialPage
    {
        Main,
        ManageApplications,
        ManageHouseholds,
        HouseholdPage,
        CreateHouseholdPage
    }
}
