using ReactiveUI;
using ShoppingList.Utils;
using System.Reactive;

namespace ShoppingList.ViewModels.Settings
{
    internal class SingleSettingViewModel(string titleKey, ReactiveCommand<Unit, Unit> command)
    {
        public string TitleKey { get; } = titleKey;
        public string Title { get; private set; } = StringProvider.GetString(titleKey);

        public ReactiveCommand<Unit, Unit> SettingCommand { get; } = command;
    }
}
