@echo off
setlocal EnableExtensions
title Brave Nightly Portable
cd /d "%~dp0"

echo.
echo  ========================================
echo   Brave Nightly Portable - Starting...
echo  ========================================
echo.

:: --- Requirement: .NET 8 Desktop Runtime (for the launcher wrapper) ---
echo [1/2] Checking requirements...
where dotnet >nul 2>&1
if errorlevel 1 goto :check_programfiles

dotnet --list-runtimes 2>nul | findstr /I /C:"Microsoft.WindowsDesktop.App 8." >nul
if not errorlevel 1 goto :launch
goto :missing_dotnet

:check_programfiles
if exist "%ProgramFiles%\dotnet\shared\Microsoft.WindowsDesktop.App\8.*" goto :launch

:missing_dotnet
echo.
echo  REQUIREMENT MISSING: .NET 8 Desktop Runtime
echo  The launcher needs it. Brave itself does not.
echo.
echo  Opening the official download page...
start "" "https://dotnet.microsoft.com/en-us/download/dotnet/8.0"
echo.
echo  Install ".NET Desktop Runtime 8.x" (x64), then run this file again.
echo.
pause
exit /b 1

:launch
echo [2/2] Launching Brave Nightly Portable...
start "" "%~dp0BraveNightlyPortable-AlexRabbit.exe" %*
exit /b 0
