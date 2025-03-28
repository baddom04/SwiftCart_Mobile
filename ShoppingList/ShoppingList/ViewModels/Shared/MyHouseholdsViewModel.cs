using ReactiveUI;
using ShoppingList.Model.Social;
using ShoppingList.Shared;
using ShoppingList.Shared.Model.Settings;
using ShoppingList.Shared.Utils;
using ShoppingList.Utils;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels.Shared
{
    internal abstract class MyHouseholdsViewModel : ViewModelBase
    {
        public virtual ObservableCollection<HouseholdListItemViewModel> MyHouseholds { get; }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            protected set { this.RaiseAndSetIfChanged(ref _isLoading, value); }
        }

        public bool EmptyMyHouseholds => MyHouseholds.Count == 0;

        protected readonly Action<ViewModelBase> _changeToPage;
        protected readonly Action<NotificationType, string> _showNotification;
        protected readonly MyHouseholdsModel _model;
        protected readonly UserAccountModel _account;

        public MyHouseholdsViewModel(UserAccountModel account, MyHouseholdsModel model, Action<ViewModelBase> changeToPage, Action<NotificationType, string> showNotification)
        {
            _account = account;
            _model = model;
            _changeToPage = changeToPage;
            _showNotification = showNotification;

            MyHouseholds = [];
            MyHouseholds.CollectionChanged += (s, e) => this.RaisePropertyChanged(nameof(EmptyMyHouseholds));
        }

        public abstract Task LoadMyHouseholds();
    }
}
