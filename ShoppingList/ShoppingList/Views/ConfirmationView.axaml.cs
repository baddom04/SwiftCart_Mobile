using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace ShoppingList.Views;

public partial class ConfirmationView : Window
{
    public bool? DialogResult { get; private set; } = null;
    public ConfirmationView(string questionKey = "Are you sure?",
                            string defaultKey = "Yes",
                            string cancelKey = "No",
                            string detailsKey = "")
    {
        InitializeComponent();
        Question.Text = questionKey;
        Details.Text = detailsKey;
        DefaultBtn.Content = defaultKey;
        CancelBtn.Content = cancelKey;
    }

    private void YesButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }

    private void NoButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}