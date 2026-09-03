# Builds the AddressPrinter installer end to end.
#
#   1. Publish a self-contained single-file AddressPrinter.exe (win-x64).
#   2. Compile the Inno Setup installer around it.
#
# Usage:
#   .\build_installer.ps1                 # full build (unsigned)
#   .\build_installer.ps1 -SkipPublish    # reuse the payload from a previous run
#
# The version comes from <Version> in AddressPrinter.csproj; nothing is hard-coded here.
#
# Signing: deliberately NOT wired up in this repo yet — no ssign.exe / certum
# code-signing folder / signtool was found on the dev machine. The installer is
# therefore built unsigned. For public releases, add signing by mirroring the
# StarTooth build_installer.ps1 (ssign.exe + ISCC SignTool= directive).

[CmdletBinding()]
param(
    [switch]$SkipPublish
)

$ErrorActionPreference = 'Stop'

# Use Win32 CommandLineToArgvW quoting when invoking native exes.
$PSNativeCommandArgumentPassing = 'Standard'

$here = $PSScriptRoot
$repo = Split-Path $here -Parent
$csproj = Join-Path $repo 'AddressPrinter.csproj'
$payloadDir = Join-Path $here 'payload'

# --- version from the csproj -------------------------------------------------
[xml]$xml = Get-Content $csproj
$version = ($xml.Project.PropertyGroup.Version | Where-Object { $_ } | Select-Object -First 1).Trim()
if (-not $version) { throw "No <Version> in $csproj" }

# Numeric quad for VersionInfoVersion: drop any -rc / -beta suffix, pad to x.y.z.0.
$numeric = ($version -split '-')[0]
while (($numeric -split '\.').Count -lt 4) { $numeric += '.0' }

Write-Host "AddressPrinter installer" -ForegroundColor Cyan
Write-Host "  display version: $version"
Write-Host "  numeric version: $numeric"

# --- 1. publish --------------------------------------------------------------
if (-not $SkipPublish) {
    Get-Process AddressPrinter -ErrorAction SilentlyContinue | Stop-Process -Force
    if (Test-Path $payloadDir) { Remove-Item $payloadDir -Recurse -Force }

    Write-Host "Publishing self-contained single-file exe..." -ForegroundColor Cyan
    dotnet publish $csproj -c Release -p:PublishSingleFile=true -r win-x64 --self-contained true -o $payloadDir --nologo
    if ($LASTEXITCODE -ne 0) { throw "dotnet publish failed" }
}

$payloadExe = Join-Path $payloadDir 'AddressPrinter.exe'
if (-not (Test-Path $payloadExe)) { throw "Payload missing: $payloadExe" }

# --- 2. compile the installer ------------------------------------------------
$iscc = $null
$candidates = @(
    "$env:LOCALAPPDATA\Programs\Inno Setup 6\ISCC.exe",
    "${env:ProgramFiles(x86)}\Inno Setup 6\ISCC.exe",
    "$env:ProgramFiles\Inno Setup 6\ISCC.exe"
)
foreach ($c in $candidates) { if (Test-Path $c) { $iscc = $c; break } }
if (-not $iscc) { throw "ISCC.exe not found. Install Inno Setup 6." }

Write-Host "Compiling installer with $iscc" -ForegroundColor Cyan
$isccArgs = @("/DMyAppVersion=$version", "/DMyAppNumericVersion=$numeric", (Join-Path $here 'AddressPrinter.iss'))
& $iscc @isccArgs
if ($LASTEXITCODE -ne 0) { throw "ISCC failed" }

$installer = Join-Path $here "output\AddressPrinter-Setup-$version.exe"
if (-not (Test-Path $installer)) { throw "Installer not produced: $installer" }

Write-Host ""
Write-Host "Done: $installer" -ForegroundColor Green
Get-Item $installer | ForEach-Object { "  {0:N1} MB" -f ($_.Length / 1MB) }