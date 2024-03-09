@echo off

call Build.bat Release

if %errorlevel% equ 0 (
    cd "DeploymentScripts"
    call Zip.bat
)
