@echo off
setlocal
cd /d "%~dp0..\.."
title Set Brave Nightly Portable as default browser — AlexRabbit
echo.
echo  Registering BraveNightlyPortable-AlexRabbit.exe as a Windows browser...
echo.
"%~dp0..\..\BraveNightlyPortable-AlexRabbit.exe" --register-default
exit /b %ERRORLEVEL%
