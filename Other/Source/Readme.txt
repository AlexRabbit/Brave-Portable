Brave Nightly Portable — Other\Source
=====================================

Manual helpers (normal users launch BraveNightlyPortable-AlexRabbit.exe).

  Set-As-Default-Browser.bat — register the AlexRabbit wrapper as Windows default browser
  update.bat                 — download latest Nightly from official GitHub
  Update-BraveNightly.ps1    — same update logic as the wrapper (PowerShell)

Default browser tip:
  Do NOT set default inside Brave Settings (that uses brave.exe → second browser).
  Use Set-As-Default-Browser.bat or:
    BraveNightlyPortable-AlexRabbit.exe --register-default
