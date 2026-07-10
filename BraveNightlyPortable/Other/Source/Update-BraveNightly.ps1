# Official Brave Nightly only — brave/brave-browser GitHub releases (win32-x64 zip).
# Verifies ProductName contains "Nightly" before installing.

param(
    [switch]$Force,
    [switch]$Quiet
)

$ErrorActionPreference = "Stop"

function Write-Log([string]$Message) { if (-not $Quiet) { Write-Host $Message } }
function Exit-Safe([int]$Code) { if ($Quiet -and $Code -ne 0) { exit 0 }; exit $Code }

$Root = $PSScriptRoot
for ($i = 0; $i -lt 8; $i++) {
    if (Test-Path (Join-Path $Root "App\AppInfo\appinfo.ini")) { break }
    $parent = Split-Path $Root -Parent
    if (-not $parent -or $parent -eq $Root) { break }
    $Root = $parent
}

$BraveDir = Join-Path $Root "App\Brave"
$BraveExe = Join-Path $BraveDir "brave.exe"
$AppInfo = Join-Path $Root "App\AppInfo\appinfo.ini"

function Get-BraveVersion([string]$Path) {
    if (-not (Test-Path $Path)) { return $null }
    $raw = (Get-Item $Path).VersionInfo.ProductVersion
    if ($raw -match "(\d+)\.(\d+)\.(\d+)\.(\d+)") { return "$($matches[2]).$($matches[3]).$($matches[4])" }
    if ($raw -match "(\d+\.\d+\.\d+)") { return $matches[1] }
    return $null
}

function Assert-NightlyExe([string]$Path) {
    $name = (Get-Item $Path).VersionInfo.ProductName
    if ($name -notmatch "Nightly") { throw "Not a Nightly build: $name" }
}

function Get-LatestNightlyFromApi() {
    $url = "https://api.github.com/repos/brave/brave-browser/releases?per_page=100"
    $releases = Invoke-RestMethod -Uri $url -Headers @{ "User-Agent" = "BraveNightlyPortable-Updater" }
    foreach ($rel in ($releases | Where-Object { $_.name -match "Nightly" })) {
        $asset = $rel.assets | Where-Object { $_.name -match "^brave-v[\d\.]+-win32-x64\.zip$" } | Select-Object -First 1
        if ($asset) {
            return [PSCustomObject]@{
                Version = $rel.tag_name.TrimStart("v")
                AssetName = $asset.name
                Url = $asset.browser_download_url
            }
        }
    }
    return $null
}

function Get-LatestNightlyFromHtml() {
    $html = (Invoke-WebRequest -Uri "https://github.com/brave/brave-browser/releases" -UseBasicParsing -Headers @{ "User-Agent" = "BraveNightlyPortable-Updater" }).Content
    foreach ($m in [regex]::Matches($html, "Nightly v(\d+\.\d+\.\d+)")) {
        $ver = $m.Groups[1].Value
        $asset = "brave-v$ver-win32-x64.zip"
        if ($html -match [regex]::Escape($asset)) {
            return [PSCustomObject]@{
                Version = $ver
                AssetName = $asset
                Url = "https://github.com/brave/brave-browser/releases/download/v$ver/$asset"
            }
        }
    }
    return $null
}

if (-not (Test-Path $BraveExe)) {
    Write-Log "First run — downloading official Brave Nightly from GitHub..."
    New-Item -ItemType Directory -Path $BraveDir -Force | Out-Null
}

$current = Get-BraveVersion $BraveExe
Write-Log "Installed Nightly: $(if ($current) { $current } else { '(none)' })"

$latest = $null
try { $latest = Get-LatestNightlyFromApi } catch { Write-Log "API failed: $($_.Exception.Message)" }
if (-not $latest) {
    try { $latest = Get-LatestNightlyFromHtml } catch { Write-Log "HTML fallback failed"; Exit-Safe 0 }
}
if (-not $latest) { Write-Log "No Nightly release found"; Exit-Safe 0 }

Write-Log "Latest official Nightly: $($latest.Version)"

if (-not $Force -and $current) {
    try {
        if ([Version]$current -ge [Version]$latest.Version) { Write-Log "Up to date"; Exit-Safe 0 }
    } catch { }
}

$zip = Join-Path $env:TEMP $latest.AssetName
$stage = Join-Path $env:TEMP ("BraveNightlyUpdate_" + [guid]::NewGuid().ToString("N"))

try {
    Write-Log "Downloading $($latest.AssetName) ..."
    Invoke-WebRequest -Uri $latest.Url -OutFile $zip -UseBasicParsing
    Get-Process brave -ErrorAction SilentlyContinue | Stop-Process -Force -ErrorAction SilentlyContinue
    Start-Sleep 1
    New-Item -ItemType Directory -Path $stage -Force | Out-Null
    Expand-Archive $zip -DestinationPath $stage -Force

    $innerExe = Get-ChildItem $stage -Recurse -Filter "brave.exe" | Select-Object -First 1
    if (-not $innerExe) { throw "No brave.exe in downloaded zip" }
    Assert-NightlyExe $innerExe.FullName

    $innerRoot = $innerExe.DirectoryName
    New-Item -ItemType Directory -Path $BraveDir -Force | Out-Null
    Get-ChildItem $BraveDir -Force -ErrorAction SilentlyContinue | Remove-Item -Recurse -Force -ErrorAction SilentlyContinue
    Copy-Item "$innerRoot\*" $BraveDir -Recurse -Force

    $parts = $latest.Version.Split(".")
    if ($parts.Count -ge 3 -and (Test-Path $AppInfo)) {
        $pkg = "$($parts[0]).$($parts[1]).$($parts[2]).0"
        (Get-Content $AppInfo) `
            -replace '^PackageVersion=.*', "PackageVersion=$pkg" `
            -replace '^DisplayVersion=.*', "DisplayVersion=$($latest.Version) Nightly" |
            Set-Content $AppInfo -Encoding ASCII
    }
    Write-Log "Updated to Nightly $($latest.Version)"
} catch {
    Write-Log "Update error: $($_.Exception.Message)"
} finally {
    Remove-Item $zip -Force -ErrorAction SilentlyContinue
    Remove-Item $stage -Recurse -Force -ErrorAction SilentlyContinue
}

Exit-Safe 0
