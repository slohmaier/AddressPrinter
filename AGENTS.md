# AddressPrinter — Projekt-Anweisungen

Gilt für jede Änderung in diesem Repo (Code, Build, Tests, Commit).
Vor jedem Tool-Call hier die verbindlichen Regeln unten lesen.

## Projekt

- **Zweck:** Druckt Adress-, Absender- und `#PORTO`-Etiketten auf einem Thermoetikettendrucker.
- **Fokus-Hardware:** Phomemo M220 (USB, 203 DPI) — ausschließlich mit diesem Modell getestet.
  Andere Drucker/Modelle sind nicht garantiert unterstützt.
- **Status:** Aktiv (Version 1.0.0)
- **Lizenz:** MIT (siehe `LICENSE`)
- **Repo:** https://github.com/slohmaier/AddressPrinter (öffentlich)

## Technologie

- **Sprache:** C# (Nullable + ImplicitUsings aktiv)
- **Framework:** .NET 10, `net10.0-windows`, **WPF** (XAML, `UseWPF=true`)
- **Abhängigkeiten:** Keine externen NuGet-Pakete
- **Zielplattform:** Windows 10 (Build 1903+) / Windows 11 (dunkle Titelleiste)

## Build & Run

Kein SCons — **`dotnet` ist das Build-System** (SDK-Projekt, `AddressPrinter.csproj`).

```powershell
dotnet build -c Debug
dotnet run
dotnet build -c Release   # Release-Build
```

- Ausgabe: `bin\Debug\net10.0-windows\AddressPrinter.exe`
- Toolchain-Pin: **.NET 10 SDK** (nur zum Bauen nötig; Runtime liefert Windows/.NET mit)

## 🔒 IRON RULES

1. **Buildartefakte niemals committen.** `bin/` und `obj/` sind per `.gitignore`
   ausgeschlossen — kein `git add -f bin obj`, kein Einchecken von `*.dll`/`*.exe`/
   `*.pdb`/generierten `*.g.cs`/`*.baml`. Working Tree immer sauber halten.
2. **Lokalisierung vollständig halten.** Neue UI-Strings müssen in **allen 8**
   Sprachdateien ergänzt werden (`Resources/Strings.*.xaml`):
   `de`, `en`, `es`, `fr`, `it`, `ja`, `nl`, `pt`. Fehlt eine Sprache, ist es ein Bug.
3. **Icon-Quelle ist `Resources/appicon.svg`.** `app.ico` wird daraus per
   `Resources/gen_icon.py` generiert (Build-Target `GenerateIcon`, benötigt
   Pillow + skia-python). `app.ico` ist committet und dient als Fallback —
   nur das SVG editieren, niemals den Build-Aufruf umgehen oder `app.ico`
   von Hand erzeugen.

## Konventionen

- **Lokalisierung:** `Localization.cs` lädt `Resources/Strings.*.xaml` je nach
  Spracheinstellung; Schlüssel in allen 8 Dateien synchron halten (siehe IRON RULE 2).
- **Einstellungen:** liegen außerhalb des Repos in `%APPDATA%\AddressPrinter\`
  (`settings.json` – Drucker, Absender, Etikettengröße, Theme, Sprache;
  `addressbook.json` – gespeicherte Empfänger). Nie ins Repo committen.
- **Print-Stack:** `PrintService.cs` kapselt den Druck (WPF-Druckpipeline an den
  Phomemo-Treiber). Änderungen am Envelope/Layout müssen mit der M220
  gegengelesen werden (Label-Größen sind mm-genau konfigurierbar).
- **Commits:** Klein, aussagekräftig, am besten auf Englisch. Keine Artefakte,
  keine Tools-/IDE-Dateien (`.vs/`, `.idea/`, `*.user`).

## Definition of Done

1. `dotnet build -c Debug` und `-c Release` laufen fehlerfrei durch.
2. Neue/geänderte Strings in allen 8 Sprachdateien vorhanden.
3. `git status` zeigt ausschließlich gewollte Quelländerungen
   (keine `bin/`-/`obj/`-Einträge, nicht mal als untracked).