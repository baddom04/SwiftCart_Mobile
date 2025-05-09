﻿using DynamicData;
using ReactiveUI;
using ShoppingList.Model.Social;
using ShoppingList.Shared;
using ShoppingList.Shared.Model.Settings;
using ShoppingList.Shared.Utils;
using ShoppingList.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels.Social
{
    internal class ManageApplicationsViewModel : ViewModelBase
    {
        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { this.RaiseAndSetIfChanged(ref _isLoading, value); }
        }

        public ObservableCollection<HouseholdApplicationViewModel> Applications { get; }
        public bool EmptyApplications => Applications.Count == 0;
        public ReactiveCommand<Unit, Unit> MainPageCommand { get; }
        public ReactiveCommand<Unit, Unit> ReloadCommand { get; }

        private readonly ManageApplicationsModel _model;
        private readonly UserAccountModel _account;
        private readonly Action<NotificationType, string> _showNotification;
        private readonly Action<SocialPage> _changePage;
        public ManageApplicationsViewModel(UserAccountModel account, ManageApplicationsModel model, Action<NotificationType, string> showNotification, Action<SocialPage> changePage)
        {
            _model = model;
            _account = account;
            _showNotification = showNotification;
            _changePage = changePage;
            Applications = [];
            MainPageCommand = ReactiveCommand.Create(() => _changePage(SocialPage.Main));
            ReloadCommand = ReactiveCommand.CreateFromTask(LoadApplicationsAsync);

            Applications.CollectionChanged += (s, e) => this.RaisePropertyChanged(nameof(EmptyApplications));
        }

        public async Task LoadApplicationsAsync()
        {
            IsLoading = true;

            try
            {
                IEnumerable<HouseholdApplicationViewModel> temp = 
                    (await _model
                    .GetAppliedToHouseholdsAsync(_account.User!.Id))
                    .Select(hh => 
                        new HouseholdApplicationViewModel(_account, new HouseholdApplicationModel(hh), _showNotification));

                Applications.Clear();
                Applications.AddRange(temp);
            }
            catch (Exception ex)
            {
                string message = $"{StringProvider.GetString("ApplicationsQueryError")}{ex.Message}";
                _showNotification(NotificationType.Error, message);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
