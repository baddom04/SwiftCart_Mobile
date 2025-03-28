using Avalonia.Controls;
using ShoppingList.ViewModels;
using System;
using System.Threading.Tasks;

namespace ShoppingList.Views
{
    public partial class MainView : UserControl
    {
        public MainView(MobileMainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
        public async Task<bool> ShowConfirmDialog(string questionKey = "ConfirmQuestion")
        {
            ConfirmationView confirmationView = new(questionKey);
            MainGrid.Children.Add(confirmationView);

            bool result = await confirmationView.ShowDialog();
            MainGrid.Children.Remove(confirmationView);

            return result;
        }
        public async Task<string?> ShowTextInputDialog(string instructionKey, Func<string, bool> validateInput)
        {
            TextInputView textInputView = new(instructionKey, validateInput);

            MainGrid.Children.Add(textInputView);
            string? result = await textInputView.ShowDialog();

            MainGrid.Children.Remove(textInputView);
            return result;
        }
    }
}
