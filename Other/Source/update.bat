@echo off
setlocal
cd /d "%~dp0"
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Update-BraveNightly.ps1" %*
if errorlevel 1 (
    echo Update failed with error %errorlevel%.
    pause
    exit /b %errorlevel%
)
echo Update finished successfully.
pause
