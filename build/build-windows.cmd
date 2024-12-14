@echo off
setlocal enabledelayedexpansion

set "version="
set "main_version="

for /f %%i in ('type .\build\version') do (
    set "version=%%i"
)

for /f %%i in ('type .\build\main_version') do (
    set "main_version=%%i"
)

mkdir .\build_out

call :build_win win-x64 x86_64
@REM call :build_win win-x86 x86
call :build_win win-arm64 aarch64

goto :eof

:build_win
echo build colordesktop-win-%main_version%%version%-%2.zip

dotnet publish .\src\ColorDesktop.Launcher -p:PublishProfile=%1

mkdir .\src\build_out\%1-dotnet\colordesktop

set "files=ColorDesktop.Api.pdb ColorDesktop.Launcher.exe ColorDesktop.Launcher.pdb av_libglesv2.dll libHarfBuzzSharp.dll libSkiaSharp.dll"

for %%f in (%files%) do (
    copy .\src\build_out\%1-dotnet\%%f .\src\build_out\%1-dotnet\colordesktop\%%f
)

echo colordesktop-win-%main_version%%version%-%2.zip build done
goto :eof