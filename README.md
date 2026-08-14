# AddressPrinter

Druckt Adress-, Absender- und #PORTO-Etiketten automatisiert auf einem Thermobondrucker.

**AddressPrinter wurde für die Phomemo-Etikettendrucker entwickelt und ausschließlich mit dem Phomemo M220 (USB, 203 DPI) getestet.** Andere Drucker oder Modelle werden nicht garantiert unterstützt.

## Funktionen

- **#PORTO-Label drucken** – druckt den großen Schriftzug `#PORTO` und darunter einen DHL-Code (den du von DHL bekommst).
- **Drucke Absender** – druckt den in den Einstellungen hinterlegten Absender.
- **Drucke Adresse** – Eingabedialog für Name, Straße, PLZ/Ort und optional Land; druckt das Empfänger-Label. Wahlweise mit „Drucken & Speichern“ direkt ins Adressbuch übernehmen.
- **Adressbuch** – wiederkehrende Empfänger speichern, auswählen und drucken oder löschen.
- **Etikettengröße** – Breite und Höhe des eingelegten Etiketts (in mm) werden in den Einstellungen konfiguriert; das Layout passt sich an.
- **Design** – hell, dunkel oder „Folge System“.
- **Lokalisierung** – 8 Sprachen: Englisch, Deutsch, Spanisch, Französisch, Italienisch, Japanisch, Niederländisch, Portugiesisch.

## Voraussetzungen

- Windows 10 (Build 1903+) oder Windows 11 (für den dunklen Titelbalken)
- [.NET 10 SDK](https://dotnet.microsoft.com/) (nur zum Bauen)
- Ein installierter Druckertreiber für den Phomemo M220 (als Windows-Drucker sichtbar)
- USB-Verbindung zum Drucker

## Bauen und Starten

```powershell
dotnet build -c Debug
dotnet run
```

Die ausführbare Datei liegt danach unter `bin\Debug\net10.0-windows\AddressPrinter.exe`.

## Konfiguration

Die Einstellungen werden als JSON unter `%APPDATA%\AddressPrinter\` gespeichert:

- `settings.json` – Drucker, Absender, Etikettengröße, Design, Sprache
- `addressbook.json` – gespeicherte Empfänger

## Wichtiger Hinweis

Dieses Programm wurde ausschließlich mit einem **Phomemo M220** (per USB) getestet. Funktionalität mit anderen Phomemo-Modellen oder anderen Thermobondruckern ist nicht garantiert.

## Lizenz

MIT – siehe [LICENSE](LICENSE).
