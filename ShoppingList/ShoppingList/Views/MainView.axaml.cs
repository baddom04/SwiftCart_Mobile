using Avalonia.Controls;
using System;
using System.Threading.Tasks;

namespace ShoppingList
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
        }
        public Task<bool> ShowConfirmDialog(string questionKey)
        {
            ConfirmationView confirmationView = new(questionKey);
            MainGrid.Children.Add(confirmationView);
            return confirmationView.ShowDialog();
        }
        public Task<string?> ShowTextInputDialog(string instructionKey, Func<string, bool> validateInput)
        {
            TextInputView textInputView = new(instructionKey, validateInput);
            MainGrid.Children.Add(textInputView);
            return textInputView.ShowDialog();
        }
    }
}
