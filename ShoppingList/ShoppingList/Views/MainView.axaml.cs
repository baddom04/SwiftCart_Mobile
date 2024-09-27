using Avalonia.Controls;
using Avalonia.Interactivity;
using ShoppingList.ViewModels;
using System;

namespace ShoppingList.Views
{
    public partial class MainView : UserControl
    {
        private Action<bool>? _dialogAction;
        public MainView()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
        private void Confirm_Click(object? sender, RoutedEventArgs e)
        {
            Click_Action(true);
        }
        private void Cancel_Click(object? sender, RoutedEventArgs e)
        {
            Click_Action(false);
        }
        public void ShowConfirmDialog(string question, Action<bool> action)
        {
            DialogQuestion.Text = question;
            _dialogAction = action;
            DialogOverlay.IsVisible = true;
        }
        private void Click_Action(bool b)
        {
            if (_dialogAction is null || !DialogOverlay.IsVisible) return;

            _dialogAction(b);
            _dialogAction = null;
            DialogOverlay.IsVisible = false;
        }
    }
}
