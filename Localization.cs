using System.Windows;

namespace AddressPrinter;

public static class Localization
{
    public static readonly string[] SupportedLanguages = { "en", "de", "es", "fr", "it", "ja", "nl", "pt" };

    public static string Current { get; private set; } = "en";

    public static void Apply(string language)
    {
        if (!SupportedLanguages.Contains(language))
            language = "en";

        Current = language;

        var dict = new ResourceDictionary
        {
            Source = new Uri($"Resources/Strings.{language}.xaml", UriKind.Relative)
        };

        var app = Application.Current;
        if (app == null) return;

        // Entferne die vorherige Sprach-Ressource, falls vorhanden.
        var existing = app.Resources.MergedDictionaries
            .FirstOrDefault(d => d.Source != null &&
                                 d.Source.OriginalString.StartsWith("Resources/Strings.", StringComparison.OrdinalIgnoreCase));
        if (existing != null)
            app.Resources.MergedDictionaries.Remove(existing);

        app.Resources.MergedDictionaries.Add(dict);
    }

    public static string Get(string key)
    {
        var app = Application.Current;
        if (app != null && app.Resources.Contains(key))
            return (string)app.Resources[key];
        return key;
    }

    public static string Get(string key, params object[] args)
    {
        return string.Format(Get(key), args);
    }
}
