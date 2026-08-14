using System.Windows;
using System.Windows.Media;

namespace AddressPrinter;

public static class ThemeManager
{
    public static string Resolve(string setting)
    {
        return setting switch
        {
            "light" => "light",
            "dark" => "dark",
            _ => IsSystemDark() ? "dark" : "light"
        };
    }

    public static bool IsSystemDark()
    {
        try
        {
            using var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            var value = key?.GetValue("AppsUseLightTheme");
            return value is int i && i == 0;
        }
        catch
        {
            return false;
        }
    }

    public static void Apply(string setting)
    {
        bool dark = Resolve(setting) == "dark";
        var app = Application.Current;
        if (app == null) return;

        var resources = app.Resources;

        Color windowBg = dark ? Color.FromRgb(0x1E, 0x1E, 0x1E) : Color.FromRgb(0xFA, 0xFA, 0xFA);
        Color controlBg = dark ? Color.FromRgb(0x2D, 0x2D, 0x30) : Color.FromRgb(0xFF, 0xFF, 0xFF);
        Color controlFg = dark ? Color.FromRgb(0xF0, 0xF0, 0xF0) : Color.FromRgb(0x1A, 0x1A, 0x1A);
        Color groupBg = dark ? Color.FromRgb(0x25, 0x25, 0x26) : Color.FromRgb(0xF0, 0xF0, 0xF0);
        Color border = dark ? Color.FromRgb(0x3F, 0x3F, 0x46) : Color.FromRgb(0xD0, 0xD0, 0xD0);
        Color accent = Color.FromRgb(0x0E, 0x63, 0x9C);

        resources["WindowBgBrush"] = new SolidColorBrush(windowBg);
        resources["ControlBgBrush"] = new SolidColorBrush(controlBg);
        resources["ControlFgBrush"] = new SolidColorBrush(controlFg);
        resources["GroupBgBrush"] = new SolidColorBrush(groupBg);
        resources["AccentBrush"] = new SolidColorBrush(accent);
        resources["BorderBrush"] = new SolidColorBrush(border);
    }
}
