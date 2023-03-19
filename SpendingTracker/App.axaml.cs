using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using SpendingTracker.ViewModels;
using SpendingTracker.Views;

namespace SpendingTracker;

public partial class App : Application
{

    private bool isStuWindow = false;

    public override void Initialize()
    {
    AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
    if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
    {

    if (!isStuWindow)
    {
    desktop.MainWindow = new MainWindow
    {
        DataContext = new MainWindowViewModel(),
    };
    }


    else
    {
    desktop.MainWindow = new StuMainWindow
    {
        DataContext = new MainViewModel(),
    };
    }

    base.OnFrameworkInitializationCompleted();
    }
    }

}