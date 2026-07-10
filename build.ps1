$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $MyInvocation.MyCommand.Path
$dotnet = Join-Path $root "tools\dotnet\dotnet.exe"
if (-not (Test-Path $dotnet)) {
    $tools = Join-Path $root "tools"
    New-Item -ItemType Directory -Path $tools -Force | Out-Null
    Invoke-WebRequest -Uri "https://dot.net/v1/dotnet-install.ps1" -OutFile "$tools\dotnet-install.ps1" -UseBasicParsing
    & "$tools\dotnet-install.ps1" -Channel 8.0 -InstallDir "$tools\dotnet" -Architecture x64
}
& $dotnet publish (Join-Path $root "wrapper\BraveNightlyPortable-AlexRabbit.csproj") -c Release -r win-x64 --self-contained true -o (Join-Path $root "wrapper\out")
Copy-Item -Force (Join-Path $root "wrapper\out\BraveNightlyPortable-AlexRabbit.exe") (Join-Path $root "BraveNightlyPortable\BraveNightlyPortable-AlexRabbit.exe")
Write-Host "Built: BraveNightlyPortable\BraveNightlyPortable-AlexRabbit.exe"
