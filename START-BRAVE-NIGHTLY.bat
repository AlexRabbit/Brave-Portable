@echo off
setlocal EnableDelayedExpansion
title Brave Nightly Portable
color 0B

:: ============================================================
::  START HERE — double-click this file to launch Brave Nightly
:: ============================================================

cd /d "%~dp0"

echo.
echo  ============================================
echo   Brave Nightly Portable - Starting...
echo  ============================================
echo.

set "PKG=%~dp0BraveNightlyPortable"
set "WRAPPER=%PKG%\BraveNightlyPortable-AlexRabbit.exe"

if not exist "%PKG%" (
    echo [ERROR] BraveNightlyPortable folder not found.
    pause
    exit /b 1
)

:: Build wrapper if missing (developers / source checkout)
if not exist "%WRAPPER%" (
    echo [INFO] First-time setup: building launcher...
    powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0build.ps1"
    if errorlevel 1 (
        echo [ERROR] Build failed. Install .NET 8 SDK or download a Release zip.
        pause
        exit /b 1
    )
)

echo [INFO] Checking for updates and launching Brave Nightly...
echo       (First run downloads ~230 MB from official GitHub — please wait)
echo.

start "" "%WRAPPER%"

exit /b 0
