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
    internal class ManageApplicationsViewModel : ViewModelBase
    {
        public ObservableCollection<HouseholdApplicationViewModel> Applications { get; }

        private readonly ManageApplicationsModel _model;
        private readonly UserAccountModel _account;
        private readonly Action<bool> _showLoading;
        private readonly Action<NotificationType, string> _showNotification;
        private readonly Action<SocialPage> _changePage;

        public bool EmptyApplications => Applications.Count == 0;
        public ReactiveCommand<Unit, Unit> MainPageCommand { get; }
        public ReactiveCommand<Unit, Unit> ReloadCommand { get; }
        
        public ManageApplicationsViewModel(UserAccountModel account, ManageApplicationsModel model, Action<bool> showLoading, Action<NotificationType, string> showNotification, Action<SocialPage> changePage)
        {
            _model = model;
            _account = account;
            _showLoading = showLoading;
            _showNotification = showNotification;
            _changePage = changePage;
            Applications = [];
            MainPageCommand = ReactiveCommand.Create(() => _changePage(SocialPage.Main));
            ReloadCommand = ReactiveCommand.CreateFromTask(LoadApplications);

            Applications.CollectionChanged += (s, e) => this.RaisePropertyChanged(nameof(EmptyApplications));
        }

        public async Task LoadApplications()
        {
            _showLoading(true);

            try
            {
                Applications.Clear();
                Applications.AddRange((await _model.GetHouseholdsAsync(_account.User!.Id)).Select(hh => new HouseholdApplicationViewModel(_account, new HouseholdApplicationModel(hh), _showNotification)));
            }
            catch (Exception ex)
            {
                string message = $"{StringProvider.GetString("ApplicationsQueryError")}{ex.Message}";
                _showNotification(NotificationType.Error, message);
            }
            finally
            {
                _showLoading(false);
            }
        }
    }
}
