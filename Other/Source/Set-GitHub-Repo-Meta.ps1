# Sets GitHub repo description + topics for AlexRabbit/Brave-Portable
# Run once after:  gh auth login
# Author: AlexRabbit

$ErrorActionPreference = "Stop"
$repo = "AlexRabbit/Brave-Portable"

$description = "Portable Brave Nightly for Windows by AlexRabbit. Double-click BraveNightlyPortable-AlexRabbit.exe — auto-updates from official GitHub, profile stays in the folder. Set as default browser with --register-default."

$topics = @(
    "brave",
    "brave-browser",
    "nightly",
    "portable",
    "portableapps",
    "windows",
    "chromium",
    "browser",
    "alexrabbit"
)

Write-Host "Updating $repo ..."
gh repo edit $repo --description $description
foreach ($t in $topics) {
    gh repo edit $repo --add-topic $t
}

Write-Host ""
Write-Host "Done. Current metadata:"
gh repo view $repo --json description,repositoryTopics,url
