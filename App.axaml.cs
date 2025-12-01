using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace Game_of_Life;

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
            var terrain = new Terrain(50, 50);
            desktop.MainWindow = new LifeForm(terrain);
        }

        base.OnFrameworkInitializationCompleted();
    }
}