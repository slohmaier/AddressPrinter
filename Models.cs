namespace AddressPrinter;

public sealed class LabelSettings
{
    public string PrinterName { get; set; } = "";
    public string SenderName { get; set; } = "";
    public string SenderStreet { get; set; } = "";
    public string SenderZipCity { get; set; } = "";
    public double LabelWidthMm { get; set; } = 70.0;
    public double LabelHeightMm { get; set; } = 36.0;
    public string Theme { get; set; } = "system";
    public string Language { get; set; } = "en";
}

public sealed class Recipient
{
    public string Name { get; set; } = "";
    public string Street { get; set; } = "";
    public string ZipCity { get; set; } = "";
    public string Country { get; set; } = "";
}
