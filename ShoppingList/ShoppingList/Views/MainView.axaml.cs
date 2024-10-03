using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using ShoppingList.ViewModels;
using System;
using System.Threading.Tasks;

namespace ShoppingList.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
        public Task<bool> ShowConfirmDialog(string question)
        {
            ConfirmationView cv = new(question);
            MainGrid.Children.Add(cv);
            return cv.ShowDialog();
        }
        public Task<string?> ShowTextInputDialog(string instruction, Func<string, bool> validateInput)
        {
            TextInputView tiv = new(instruction, validateInput);
            MainGrid.Children.Add(tiv);
            return tiv.ShowDialog();
        }

        private void ListBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
        {
            if (sender is not ListBox listBox) return;
            foreach (var item in listBox.GetLogicalChildren())
            {
                var vmi2 = (item as ListBoxItem)!.Content;
                if(vmi2 == listBox.SelectedItem)
                {
                    PathIcon vmi = (item.GetLogicalChildren() as PathIcon)!;
                }
            }
        }
    }
}
