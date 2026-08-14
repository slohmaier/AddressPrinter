using System.Windows;

namespace AddressPrinter;

public partial class AddressBookDialog : Window
{
    private readonly List<Recipient> _recipients;

    public AddressBookDialog()
    {
        InitializeComponent();
        Loaded += (_, _) => ThemeHelper.SetDarkTitleBar(this);
        _recipients = SettingsService.LoadAddressBook();
        RefreshList();
    }

    private void RefreshList()
    {
        AddressListBox.ItemsSource = null;
        AddressListBox.ItemsSource = _recipients;
    }

    public Recipient? SelectedRecipient =>
        AddressListBox.SelectedItem as Recipient;

    private void PrintButton_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedRecipient is not Recipient recipient)
        {
            MessageBox.Show(this,
                Localization.Get("AddressBook.NoSelectionMsg"),
                Localization.Get("AddressBook.NoSelectionTitle"),
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            return;
        }

        var main = Owner as MainWindow;
        var settings = main?.Settings ?? new LabelSettings();
        PrintService.PrintRecipientLabel(settings.PrinterName, settings, recipient);
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedRecipient is not Recipient recipient)
        {
            MessageBox.Show(this,
                Localization.Get("AddressBook.NoSelectionMsg"),
                Localization.Get("AddressBook.NoSelectionTitle"),
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            return;
        }

        var result = MessageBox.Show(this,
            Localization.Get("AddressBook.DeleteConfirm", recipient.Name),
            Localization.Get("AddressBook.DeleteTitle"),
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
            _recipients.Remove(recipient);
            SettingsService.SaveAddressBook(_recipients);
            RefreshList();
        }
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
