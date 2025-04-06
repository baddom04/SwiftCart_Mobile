using Avalonia.Controls;
using System.Threading.Tasks;
using System;

namespace ShoppingListEditor.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public async Task<bool> ShowConfirmDialogAsync(string questionKey = "ConfirmQuestion")
        {
            ConfirmationView confirmationView = new(questionKey);
            MainGrid.Children.Add(confirmationView);

            bool result = await confirmationView.ShowDialog();
            MainGrid.Children.Remove(confirmationView);

            return result;
        }
        public async Task<string?> ShowTextInputDialogAsync(string instructionKey, Func<string, bool> validateInput)
        {
            TextInputView textInputView = new(instructionKey, validateInput);

            MainGrid.Children.Add(textInputView);
            string? result = await textInputView.ShowDialog();

            MainGrid.Children.Remove(textInputView);
            return result;
        }
    }
}