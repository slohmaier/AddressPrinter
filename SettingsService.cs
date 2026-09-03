using System.IO;
using System.Text.Json;

namespace AddressPrinter;

public static class SettingsService
{
    private static readonly string ConfigDir = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AddressPrinter");

    private static readonly string ConfigPath = Path.Combine(ConfigDir, "settings.json");
    private static readonly string AddressBookPath = Path.Combine(ConfigDir, "addressbook.json");

    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public static LabelSettings LoadSettings()
    {
        try
        {
            if (File.Exists(ConfigPath))
                return JsonSerializer.Deserialize<LabelSettings>(File.ReadAllText(ConfigPath)) ?? new LabelSettings();
        }
        catch
        {
            // On errors, continue with default values.
        }
        return new LabelSettings();
    }

    public static void SaveSettings(LabelSettings settings)
    {
        Directory.CreateDirectory(ConfigDir);
        File.WriteAllText(ConfigPath, JsonSerializer.Serialize(settings, JsonOptions));
    }

    public static List<Recipient> LoadAddressBook()
    {
        try
        {
            if (File.Exists(AddressBookPath))
                return JsonSerializer.Deserialize<List<Recipient>>(File.ReadAllText(AddressBookPath)) ?? new List<Recipient>();
        }
        catch
        {
            // On errors, continue with an empty list.
        }
        return new List<Recipient>();
    }

    public static void SaveAddressBook(List<Recipient> recipients)
    {
        Directory.CreateDirectory(ConfigDir);
        File.WriteAllText(AddressBookPath, JsonSerializer.Serialize(recipients, JsonOptions));
    }
}
