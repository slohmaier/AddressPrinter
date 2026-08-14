using System.Windows;

namespace AddressPrinter;

public partial class PortoDialog : Window
{
    public string Code { get; private set; } = "";

    public PortoDialog()
    {
        InitializeComponent();
        Loaded += (_, _) => ThemeHelper.SetDarkTitleBar(this);
        CodeBox.Focus();
    }

    private void PrintButton_Click(object sender, RoutedEventArgs e)
    {
        Code = CodeBox.Text.Trim();
        DialogResult = true;
    }
}
