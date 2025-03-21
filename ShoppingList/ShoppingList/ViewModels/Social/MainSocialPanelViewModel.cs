﻿using ShoppingList.Core;
using ShoppingList.Model.Settings;
using ShoppingList.Model.Social;
using ShoppingList.Utils;
using ShoppingList.ViewModels.Shared;
using System;
using System.Collections.Generic;

namespace ShoppingList.ViewModels.Social
{
    internal class MainSocialPanelViewModel : MainViewModelBase<SocialPage>
    {
        private readonly CreateHouseholdViewModel _householdEditingPage;

        public MainSocialPanelViewModel(MyHouseholdsModel householdsModel, UserAccountModel account, Action<NotificationType, string> showNotification, Action<bool> showLoading)
        {
            _householdEditingPage = new CreateHouseholdViewModel(new CreateHouseholdModel(), ChangePage, showLoading);
            _pages = new Dictionary<SocialPage, ViewModelBase>()
            {
                { SocialPage.Main, new SocialPanelViewModel(new SocialPanelModel(), showNotification, ChangePage) },
                { SocialPage.ManageApplications, new ManageApplicationsViewModel(account, new ManageApplicationsModel(), showNotification, ChangePage) },
                { SocialPage.ManageHouseholds, new ManageHouseholdsViewModel(account, householdsModel, ChangePage, showNotification, ChangeToHouseholdPage, HouseholdEditingPage, showLoading) },
                { SocialPage.CreateHouseholdPage, _householdEditingPage },
            };
            _currentPage = _pages[SocialPage.Main];
        }
        private void ChangeToHouseholdPage(ViewModelBase householdViewModel)
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
