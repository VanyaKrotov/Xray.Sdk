@echo off
setlocal enabledelayedexpansion

set SRC_DIR=%~dp0
set OUT_DIR=%~dp0..\%1

if not exist "%OUT_DIR%" (
    mkdir "%OUT_DIR%"
)

echo Building Go shared library for current system → %OUT_DIR%

set OS=%OS%
set ARCH=%PROCESSOR_ARCHITECTURE%

if /I "%OS%"=="Windows_NT" (
    echo Build Windows %ARCH%
    go build -buildmode=c-shared -o "%OUT_DIR%\NativeWrapper.dll" -trimpath -ldflags "-s -w -buildid=" "%SRC_DIR%\main.go"
) else (
    echo Unsupported OS: %OS%
    exit /b 1
)

echo Build complete. Artifact in %OUT_DIR%:
dir /b "%OUT_DIR%"

endlocal
