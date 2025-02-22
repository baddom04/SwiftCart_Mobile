using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Media;
using ShoppingList.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingList
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            Loaded += (_, _) => SetMenuIconColors(MainMenu);
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

        private void ListBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (sender is not ListBox listBox) return;

            SetMenuIconColors(listBox);
        }
        private static void SetMenuIconColors(ListBox listBox)
        {
            foreach (var item in listBox.GetLogicalChildren())
            {
                if ((item as ListBoxItem)!.Content == listBox.SelectedItem)
                {
                    SetColorToPathIcon(item, "MainBtnBG");
                }
                else
                {
                    SetColorToPathIcon(item, "MainFG");
                }
            }
        }
        private static void SetColorToPathIcon(ILogical item, string resKey)
        {
            SolidColorBrush? brush = Application.Current!.FindResource(resKey) as SolidColorBrush;
            if (brush is null) return;

            (item.GetLogicalChildren().First() as PathIcon)!.Foreground = brush;
        }
    }
}
