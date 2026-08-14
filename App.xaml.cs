using System.Windows;

namespace AddressPrinter;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var settings = SettingsService.LoadSettings();
        Localization.Apply(settings.Language);
        ThemeManager.Apply(settings.Theme);
    }
}
