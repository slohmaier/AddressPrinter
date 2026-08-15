# AddressPrinter

Automatically prints address, sender, and #PORTO labels on a thermal label printer.

**AddressPrinter was built for Phomemo label printers and was exclusively tested with the Phomemo M220 (USB, 203 DPI).** Other printers or models are not guaranteed to be supported.

## Features

- **Print #PORTO** – prints the large `#PORTO` headline with a DHL code underneath.
- **Print sender** – prints the sender address configured in settings.
- **Print address** – input dialog for name, street, post code/city, and optional country; prints the recipient label. Optionally save to the address book via "Print & Save".
- **Address book** – store, select, print or delete recurring recipients.
- **Label size** – width and height of the loaded label (in mm) configured in settings; the layout adapts automatically.
- **Theme** – light, dark, or follow system.
- **Localisation** – 8 languages: English, German, Spanish, French, Italian, Japanese, Dutch, Portuguese.

## Requirements

- Windows 10 (Build 1903+) or Windows 11 (for the dark title bar)
- [.NET 10 SDK](https://dotnet.microsoft.com/) (build only)
- An installed Phomemo M220 printer driver (visible as a Windows printer)
- USB connection to the printer

## Build and Run

```powershell
dotnet build -c Debug
dotnet run
```

The executable will be at `bin\Debug\net10.0-windows\AddressPrinter.exe`.

## Configuration

Settings are stored as JSON in `%APPDATA%\AddressPrinter\`:

- `settings.json` – printer, sender, label size, theme, language
- `addressbook.json` – saved recipients

## Important Note

This software has been tested exclusively with a **Phomemo M220** (via USB). Functionality with other Phomemo models or other thermal label printers is not guaranteed.

## License

MIT – see [LICENSE](LICENSE).