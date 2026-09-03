; AddressPrinter Inno Setup Script
;
; Standard wizard: Welcome / License / Directory / StartMenu / Tasks / Ready / Finish.
;
; Adapted from the StarTooth installer (which itself follows MouseCross/ControlNav),
; minus everything those carry for their uiAccess / assistive-technology nature:
; no AT registration, no uiAccess launch dance. AddressPrinter is an ordinary
; desktop app.
;
; No autostart handling: AddressPrinter has no autostart feature.
;
; Version is passed in via ISCC.exe /DMyAppVersion=X.Y.Z (build_installer.ps1 reads it
; from the .csproj). VersionInfoVersion needs a plain numeric quad, passed separately.

#ifndef MyAppVersion
  #define MyAppVersion "0.0.0"
#endif
#ifndef MyAppNumericVersion
  #define MyAppNumericVersion "0.0.0.0"
#endif

#define MyAppName       "AddressPrinter"
#define MyAppPublisher  "Stefan Lohmaier"
#define MyAppURL        "https://github.com/slohmaier/AddressPrinter"
#define MyAppExeName    "AddressPrinter.exe"
#define MyAppId         "{{6F2E9C41-A1B7-4D3E-8C50-9D8B4E7A2F60}"

[Setup]
AppId={#MyAppId}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
VersionInfoVersion={#MyAppNumericVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}/releases
AppCopyright=Copyright (C) 2026 Stefan Lohmaier

DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=no
DisableDirPage=no
DisableReadyPage=no
DisableWelcomePage=no
DisableFinishedPage=no
AllowNoIcons=yes

; Per-machine install: all-users Start Menu entry and a clean Programs & Features
; uninstall. No uiAccess here, so Program Files is a convention, not a requirement.
PrivilegesRequired=admin
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible

; A running instance is closed by the installer in [Code] PrepareToInstall, so the user
; never sees a "please close the application" prompt.
CloseApplications=no
RestartApplications=no

Compression=lzma2/ultra64
SolidCompression=yes
OutputDir=output
OutputBaseFilename={#MyAppName}-Setup-{#MyAppVersion}
SetupIconFile=..\app.ico
UninstallDisplayIcon={app}\{#MyAppExeName}
UninstallDisplayName={#MyAppName} {#MyAppVersion}
WizardStyle=modern

LicenseFile=..\LICENSE

[Languages]
Name: "en"; MessagesFile: "compiler:Default.isl"
Name: "de"; MessagesFile: "compiler:Languages\German.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
; Self-contained single-file build produced by build_installer.ps1. WPF still
; ships a few native *_cor3.dll companions next to the single-file exe, so the
; whole payload folder is installed.
Source: "payload\*";                 DestDir: "{app}"; Flags: ignoreversion recursesubdirs
Source: "..\LICENSE";              DestDir: "{app}"; DestName: "LICENSE.txt";   Flags: ignoreversion
Source: "..\README.md";            DestDir: "{app}"; DestName: "README.txt";    Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}";                       Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}";               Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
; runasoriginaluser drops the installer's elevated token back to the logged-in user before
; launching, so the app does not run elevated.
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#MyAppName}}"; \
    Flags: nowait postinstall skipifsilent runasoriginaluser

[UninstallRun]
Filename: "{cmd}"; Parameters: "/C taskkill /F /IM {#MyAppExeName} 2>nul"; \
    Flags: runhidden; RunOnceId: "KillAddressPrinter"

[Code]
// Close any running AddressPrinter so its files aren't locked during install.
procedure CloseRunningInstance;
var
  killRc: Integer;
begin
  Exec(ExpandConstant('{sys}\taskkill.exe'), '/F /IM {#MyAppExeName}', '',
       SW_HIDE, ewWaitUntilTerminated, killRc);
  Sleep(400);
end;

function PrepareToInstall(var NeedsRestart: Boolean): String;
begin
  CloseRunningInstance;
  Result := '';
end;