using Avalonia.Controls;
using SpendingTracker.Interfaces;

namespace SpendingTracker.Views;

public partial class MainWindow : Window, IView
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public void ClearTextBox()
    {
        SpendingTextBox.Clear();
    }
}