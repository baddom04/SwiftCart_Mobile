using ReactiveUI;
using ShoppingList.Core;
using ShoppingList.Core.Enums;
using ShoppingList.Model.Social;
using ShoppingList.Shared.Utils;
using ShoppingList.Utils;
using ShoppingList.ViewModels.Shared;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels.Social
{
    internal class HouseholdSearchResultViewModel : HouseholdListItemViewModel
    {
        private HouseholdRelationship _relationship;
        public HouseholdRelationship Relationship
        {
            get { return _relationship; }
            private set { this.RaiseAndSetIfChanged(ref _relationship, value); }
        }

        public override ReactiveCommand<Unit, Unit> HouseholdOperationCommand { get; }

        private readonly HouseholdListItemModel _model;
        private readonly Action<NotificationType, string> _showNotification;
        public HouseholdSearchResultViewModel(HouseholdListItemModel model, Action<NotificationType, string> showNotification) : base()
        {
            HouseholdOperationCommand = ReactiveCommand.CreateFromTask(ApplyAsync);
            _model = model;
            _showNotification = showNotification;

            OnHouseholdChanged();
            _model.HouseholdChanged += OnHouseholdChanged;
        }

        private void OnHouseholdChanged()
        {
            Name = _model.Household.Name;
            Identifier = _model.Household.Identifier;
            Relationship = _model.Household.Relationship!.Value;
        }

        private async Task ApplyAsync()
        {
            IsLoading = true;

            try
            {
                await _model.ApplyAsync();
                await _model.UpdateRelationshipAsync();
                this.RaisePropertyChanged(nameof(Household));
            }
            catch (Exception ex)
            {
                string message = $"{StringProvider.GetString("ApplyError")}{ex.Message}";
                _showNotification(NotificationType.Error, message);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
