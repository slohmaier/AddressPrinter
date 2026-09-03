# AddressPrinter — Project Instructions

Applies to every change in this repository (code, build, tests, commit).
Read the binding rules below before any tool call.

## Project

- **Purpose:** Prints address, sender and `#PORTO` labels on a thermal label printer.
- **Target hardware:** Phomemo M220 (USB, 203 DPI) — tested exclusively with this model.
  Other printers/models are not guaranteed to be supported.
- **Status:** Active (version 1.0.0)
- **License:** MIT (see `LICENSE`)
- **Repo:** https://github.com/slohmaier/AddressPrinter (public)

## Technology

- **Language:** C# (Nullable + ImplicitUsings enabled)
- **Framework:** .NET 10, `net10.0-windows`, **WPF** (XAML, `UseWPF=true`)
- **Dependencies:** None (no external NuGet packages)
- **Target platform:** Windows 10 (Build 1903+) / Windows 11 (dark title bar)

## Build & Run

No SCons — **`dotnet` is the build system** (SDK-style project, `AddressPrinter.csproj`).

```powershell
dotnet build -c Debug
dotnet run
dotnet build -c Release   # Release build
```

- Output: `bin\Debug\net10.0-windows\AddressPrinter.exe`
- Toolchain pin: **.NET 10 SDK** (required for building only; runtime ships with Windows/.NET)

## 🔒 IRON RULES

1. **Never commit build artifacts.** `bin/` and `obj/` are excluded via `.gitignore` —
   no `git add -f bin obj`, no checking in `*.dll`/`*.exe`/`*.pdb`/generated
   `*.g.cs`/`*.baml`. Always keep the working tree clean.
2. **Keep localization complete.** New UI strings must be added to **all 8** language
   files (`Resources/Strings.*.xaml`): `de`, `en`, `es`, `fr`, `it`, `ja`, `nl`, `pt`.
   A missing language is a bug.
3. **The icon source is `Resources/appicon.svg`.** `app.ico` is generated from it via
   `Resources/gen_icon.py` (build target `GenerateIcon`, requires Pillow + skia-python).
   `app.ico` is committed and serves as fallback — only ever edit the SVG, never
   circumvent the build step or generate `app.ico` by hand.

## Conventions

- **Localization:** `Localization.cs` loads `Resources/Strings.*.xaml` depending on the
  language setting; keep keys in sync across all 8 files (see IRON RULE 2).
- **Settings:** live outside the repo in `%APPDATA%\AddressPrinter\`
  (`settings.json` – printer, sender, label size, theme, language;
  `addressbook.json` – saved recipients). Never commit them to the repo.
- **Print stack:** `PrintService.cs` encapsulates printing (WPF print pipeline to the
  Phomemo driver). Changes to the layout/envelope must be validated against the M220
  (label sizes are configurable in mm).
- **Commits:** Small and meaningful, in English. No artifacts or IDE/tool files
  (`.vs/`, `.idea/`, `*.user`).

## Definition of Done

1. `dotnet build -c Debug` and `-c Release` succeed without errors.
2. New/changed strings exist in all 8 language files.
3. `git status` shows only intended source changes (no `bin/`/`obj/` entries,
   not even untracked).