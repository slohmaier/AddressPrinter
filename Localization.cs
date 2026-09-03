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

        // Add the new resource first, then remove the old one,
        // so DynamicResource bindings always find a match.
        var existing = app.Resources.MergedDictionaries
            .FirstOrDefault(d => d.Source != null &&
                                 d.Source.OriginalString.StartsWith("Resources/Strings.", StringComparison.OrdinalIgnoreCase));

        app.Resources.MergedDictionaries.Add(dict);

        if (existing != null)
            app.Resources.MergedDictionaries.Remove(existing);
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
