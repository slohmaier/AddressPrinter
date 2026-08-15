using System.Windows;

namespace AddressPrinter;

public partial class AddressDialog : Window
{
    public Recipient? Result { get; private set; }
    public bool SaveToBook { get; private set; }

    public AddressDialog()
    {
        InitializeComponent();
        Loaded += OnLoaded;
        NameBox.Focus();
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

    private bool ValidateAndBuild()
    {
        if (string.IsNullOrWhiteSpace(NameBox.Text) ||
            string.IsNullOrWhiteSpace(StreetBox.Text) ||
            string.IsNullOrWhiteSpace(ZipCityBox.Text))
        {
            MessageBox.Show(this,
                Localization.Get("Address.ValidationMsg"),
                Localization.Get("Address.ValidationTitle"),
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return false;
        }

        Result = new Recipient
        {
            Name = NameBox.Text.Trim(),
            Street = StreetBox.Text.Trim(),
            ZipCity = ZipCityBox.Text.Trim(),
            Country = CountryBox.Text.Trim()
        };
        return true;
    }

    private void PrintButton_Click(object sender, RoutedEventArgs e)
    {
        if (ValidateAndBuild())
        {
            SaveToBook = false;
            DialogResult = true;
        }
    }

    private void SaveAndPrintButton_Click(object sender, RoutedEventArgs e)
    {
        if (ValidateAndBuild())
        {
            SaveToBook = true;
            DialogResult = true;
        }
    }
}