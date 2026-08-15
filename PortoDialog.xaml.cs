using System.Windows;

namespace AddressPrinter;

public partial class PortoDialog : Window
{
    public string Code { get; private set; } = "";

    public PortoDialog()
    {
        InitializeComponent();
        Loaded += OnLoaded;
        CodeBox.Focus();
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        bool dark = ThemeManager.Resolve(SettingsService.LoadSettings().Theme) == "dark";
        ThemeHelper.SetDarkTitleBar(this, dark);
        ThemeManager.ThemeApplied += ReapplyTheme;
        Closed += (_, _) => ThemeManager.ThemeApplied -= ReapplyTheme;
    }

    private void ReapplyTheme()
    {
        bool dark = ThemeManager.Resolve(SettingsService.LoadSettings().Theme) == "dark";
        ThemeHelper.SetDarkTitleBar(this, dark);
    }

    private void PrintButton_Click(object sender, RoutedEventArgs e)
    {
        Code = CodeBox.Text.Trim();
        DialogResult = true;
    }
}