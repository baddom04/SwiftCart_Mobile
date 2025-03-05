using ReactiveUI;
using ShoppingList.Utils;
using System;
using System.Reactive;

namespace ShoppingList.ViewModels.Social
{
    internal class HouseholdApplicationViewModel : ViewModelBase
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

        public ReactiveCommand<Unit, Unit> DeleteCommand { get; }

        private readonly Action<NotificationType, string> _showNotification;

        public HouseholdApplicationViewModel()
        {
            
        }
    }
}