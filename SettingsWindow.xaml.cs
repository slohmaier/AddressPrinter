using System.Windows;
using System.Windows.Controls;

namespace AddressPrinter;

public partial class SettingsWindow : Window
{
    private readonly LabelSettings _settings;

    public SettingsWindow(LabelSettings settings)
    {
        InitializeComponent();
        Loaded += (_, _) => ThemeHelper.SetDarkTitleBar(this);
        _settings = settings;

        PrinterCombo.ItemsSource = PrintService.GetPrinters();
        PrinterCombo.Text = settings.PrinterName;

        foreach (ComboBoxItem item in ThemeCombo.Items)
        {
            if ((string)item.Tag == settings.Theme)
            {
                ThemeCombo.SelectedItem = item;
                break;
            }
        }

        foreach (ComboBoxItem item in LanguageCombo.Items)
        {
            if ((string)item.Tag == settings.Language)
            {
                LanguageCombo.SelectedItem = item;
                break;
            }
        }

        SenderNameBox.Text = settings.SenderName;
        SenderStreetBox.Text = settings.SenderStreet;
        SenderZipCityBox.Text = settings.SenderZipCity;
        WidthBox.Text = settings.LabelWidthMm.ToString("0.#");
        HeightBox.Text = settings.LabelHeightMm.ToString("0.#");
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        _settings.PrinterName = PrinterCombo.Text;
        _settings.SenderName = SenderNameBox.Text.Trim();
        _settings.SenderStreet = SenderStreetBox.Text.Trim();
        _settings.SenderZipCity = SenderZipCityBox.Text.Trim();

        if (double.TryParse(WidthBox.Text, out var w) && w > 0)
            _settings.LabelWidthMm = w;
        if (double.TryParse(HeightBox.Text, out var h) && h > 0)
            _settings.LabelHeightMm = h;

        _settings.Theme = (ThemeCombo.SelectedItem as ComboBoxItem)?.Tag as string ?? "system";
        _settings.Language = (LanguageCombo.SelectedItem as ComboBoxItem)?.Tag as string ?? "en";

        SettingsService.SaveSettings(_settings);
        ThemeManager.Apply(_settings.Theme);
        Localization.Apply(_settings.Language);
        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}
