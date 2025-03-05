using Avalonia.Markup.Xaml.MarkupExtensions;
using ReactiveUI;
using ShoppingList.Core;
using ShoppingList.Core.Enums;
using ShoppingList.Model.Social;
using ShoppingList.Utils;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels.Social
{
    internal class HouseholdListItemViewModel : ViewModelBase
    {
        private string _name = string.Empty;
        public string Name
        {
            get { return _name; }
            private set { this.RaiseAndSetIfChanged(ref _name, value); }
        }

        private string _identifier = string.Empty;
        public string Identifier 
        {
            get { return _identifier; }
            private set { this.RaiseAndSetIfChanged(ref _identifier, value); }
        }

        private HouseholdRelationship _relationship;
        public HouseholdRelationship Relationship
        {
            get { return _relationship; }
            private set { this.RaiseAndSetIfChanged(ref _relationship, value); }
        }

        public ReactiveCommand<Unit, Unit> ApplyCommand { get; }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { this.RaiseAndSetIfChanged(ref _isLoading, value); }
        }

        private readonly HouseholdListItemModel _model;
        private readonly Action<NotificationType, string> _showNotification;
        public HouseholdListItemViewModel(HouseholdListItemModel model, Action<NotificationType, string> showNotification)
        {
            ApplyCommand = ReactiveCommand.CreateFromTask(Apply);
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

        private async Task Apply()
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
