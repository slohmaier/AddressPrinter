using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;

namespace AddressPrinter;

public static class PrintService
{
    public static IReadOnlyList<string> GetPrinters()
    {
        try
        {
            using var server = new LocalPrintServer();
            return server.GetPrintQueues()
                         .Select(q => q.FullName)
                         .OrderBy(n => n)
                         .ToList();
        }
        catch
        {
            return Array.Empty<string>();
        }
    }

    public static void PrintSenderLabel(string printerName, LabelSettings settings)
    {
        var doc = BuildAddressLabelDocument(
            settings,
            settings.SenderName,
            settings.SenderStreet,
            settings.SenderZipCity);
        PrintDocument(printerName, doc, settings, "Sender");
    }

    public static void PrintRecipientLabel(string printerName, LabelSettings settings, Recipient r)
    {
        var doc = BuildAddressLabelDocument(
            settings,
            r.Name,
            r.Street,
            r.ZipCity,
            r.Country);
        PrintDocument(printerName, doc, settings, "Address");
    }

    public static void PrintPortoLabel(string printerName, LabelSettings settings, string code)
    {
        var doc = BuildPortoLabelDocument(settings, code);
        PrintDocument(printerName, doc, settings, "#PORTO");
    }

    private static void PrintDocument(string printerName, FixedDocument doc, LabelSettings settings, string jobName)
    {
        var dialog = new PrintDialog();

        if (!string.IsNullOrWhiteSpace(printerName))
        {
            try
            {
                using var server = new LocalPrintServer();
                var queue = server.GetPrintQueue(printerName);
                if (queue != null)
                {
                    dialog.PrintQueue = queue;
                }
                else
                {
                    printerName = "";
                }
            }
            catch
            {
                printerName = "";
            }
        }

        if (string.IsNullOrWhiteSpace(printerName))
        {
            if (dialog.ShowDialog() != true)
                return;
        }

        dialog.PrintTicket.PageMediaSize = new PageMediaSize(
            settings.LabelWidthMm / 25.4 * 96,
            settings.LabelHeightMm / 25.4 * 96);

        dialog.PrintDocument(doc.DocumentPaginator, $"AddressPrinter – {jobName}");
    }

    private static FixedDocument BuildAddressLabelDocument(LabelSettings settings, params string[] lines)
    {
        double wMm = Math.Max(settings.LabelWidthMm, 30);
        double hMm = Math.Max(settings.LabelHeightMm, 20);
        double w = wMm / 25.4 * 96;
        double h = hMm / 25.4 * 96;

        var page = new FixedPage { Width = w, Height = h };
        double font = w / 10.7;
        double margin = w / 20.0;
        double lineHeight = font * 1.35;

        string text = string.Join("\n", lines.Where(l => !string.IsNullOrWhiteSpace(l)));

        var block = new TextBlock
        {
            Text = text,
            FontFamily = new FontFamily("Arial"),
            FontSize = font,
            Foreground = Brushes.Black,
            TextWrapping = TextWrapping.Wrap,
            Width = w - 2 * margin,
            LineHeight = lineHeight,
            LineStackingStrategy = LineStackingStrategy.BlockLineHeight
        };
        FixedPage.SetLeft(block, margin);
        FixedPage.SetTop(block, margin);
        page.Children.Add(block);

        return ToFixedDocument(page, w, h);
    }

    private static FixedDocument BuildPortoLabelDocument(LabelSettings settings, string code)
    {
        double wMm = Math.Max(settings.LabelWidthMm, 30);
        double hMm = Math.Max(settings.LabelHeightMm, 20);
        double w = wMm / 25.4 * 96;
        double h = hMm / 25.4 * 96;

        var page = new FixedPage { Width = w, Height = h };

        page.Children.Add(Text("#PORTO", w / 5.0, 0, 0, Brushes.Black, w));

        if (!string.IsNullOrWhiteSpace(code))
        {
            page.Children.Add(Text(code, w / 8.0, w / 12.0, w / 5.7, Brushes.Black, w - w / 6.0));
        }

        return ToFixedDocument(page, w, h);
    }

    private static TextBlock Text(string value, double size, double x, double y, Brush color, double maxWidth)
    {
        var tb = new TextBlock
        {
            Text = value,
            FontFamily = new FontFamily("Arial"),
            FontSize = size,
            Foreground = color,
            TextWrapping = TextWrapping.Wrap,
            Width = maxWidth
        };
        FixedPage.SetLeft(tb, x);
        FixedPage.SetTop(tb, y);
        return tb;
    }

    private static FixedDocument ToFixedDocument(FixedPage page, double width, double height)
    {
        var doc = new FixedDocument();
        var pageContent = new PageContent();
        ((IAddChild)pageContent).AddChild(page);
        doc.Pages.Add(pageContent);
        doc.DocumentPaginator.PageSize = new Size(width, height);
        return doc;
    }
}