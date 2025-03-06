using ReactiveUI;
using ShoppingList.Model.Social;
using ShoppingList.Utils;
using System;
using System.Collections.ObjectModel;
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

        public ReactiveCommand<Unit, Unit> GoBackCommand { get; }

        private readonly Action<SocialPage> _changePage;
        private readonly Action<NotificationType, string> _showNotification;
        private readonly ManageHouseholdsModel _model;
        public ManageHouseholdsViewModel(ManageHouseholdsModel model, Action<SocialPage> changePage, Action<NotificationType, string> showNotification)
        {
            _changePage = changePage;
            _model = model;
            _showNotification = showNotification;
            GoBackCommand = ReactiveCommand.Create(() => _changePage(SocialPage.Main));

            MyHouseholds = [];
        }

        public async Task LoadMyHouseholds()
        {
            IsLoading = true;

            try
            {
                await Task.Delay(1000);
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
