using System.Windows;

namespace AddressPrinter;

public partial class MainWindow : Window
{
    private readonly LabelSettings _settings;

    public LabelSettings Settings => _settings;

    public MainWindow()
    {
        InitializeComponent();
        Loaded += (_, _) => ThemeHelper.SetDarkTitleBar(this);
        _settings = SettingsService.LoadSettings();
        RefreshPrinterList();
    }

    private void RefreshPrinterList()
    {
        PrinterCombo.ItemsSource = PrintService.GetPrinters();
        PrinterCombo.Text = _settings.PrinterName;
    }

    private void PrintPortoButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new PortoDialog();
        if (dialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(dialog.Code))
        {
            PrintService.PrintPortoLabel(_settings.PrinterName, _settings, dialog.Code);
        }
    }

    private void PrintSenderButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_settings.SenderName))
        {
            MessageBox.Show(this,
                Localization.Get("MainWindow.SenderMissingMsg"),
                Localization.Get("MainWindow.SenderMissingTitle"),
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            return;
        }

        PrintService.PrintSenderLabel(_settings.PrinterName, _settings);
    }

    private void PrintAddressButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new AddressDialog { Owner = this };
        if (dialog.ShowDialog() == true && dialog.Result is Recipient recipient)
        {
            if (dialog.SaveToBook)
            {
                var book = SettingsService.LoadAddressBook();
                book.Add(recipient);
                SettingsService.SaveAddressBook(book);
            }

            PrintService.PrintRecipientLabel(_settings.PrinterName, _settings, recipient);
        }
    }

    private void PrintFromBookButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new AddressBookDialog { Owner = this };
        dialog.ShowDialog();
    }

    private void SettingsButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new SettingsWindow(_settings) { Owner = this };
        if (dialog.ShowDialog() == true)
        {
            RefreshPrinterList();
        }
    }
}
