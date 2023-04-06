using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using SpendingTracker.ViewModels;
using SpendingTracker.Views;

namespace SpendingTracker;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
            MainWindowViewModel viewModel = new MainWindowViewModel();
            viewModel.View = (MainWindow)desktop.MainWindow;
            desktop.MainWindow.DataContext = viewModel; //TODO: looks strange, maybe we can inject it better.
        }

        base.OnFrameworkInitializationCompleted();
    }
}