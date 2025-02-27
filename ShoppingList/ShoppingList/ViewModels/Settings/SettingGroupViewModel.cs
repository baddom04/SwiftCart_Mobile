using ReactiveUI;
using ShoppingList.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;

namespace ShoppingList.ViewModels.Settings
{
    internal class SettingGroupViewModel : ViewModelBase
    {
        public string TitleKey { get; }
        public string Title { get; private set; }

        private bool _isOpen = true;
        public bool IsOpen
        {
            get { return _isOpen; }
            set { this.RaiseAndSetIfChanged(ref _isOpen, value); }
        }
        public ReactiveCommand<Unit, bool> OpenCommand { get; }

        public ObservableCollection<SingleSettingViewModel> Settings { get; }

        public SettingGroupViewModel(string titleKey, IEnumerable<SingleSettingViewModel> settings)
        {
            TitleKey = titleKey;
            Title = StringProvider.GetString(titleKey);

            OpenCommand = ReactiveCommand.Create(() => IsOpen = !IsOpen);

            Settings = [.. settings];
        }
    }
}
