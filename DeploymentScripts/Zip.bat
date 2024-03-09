@echo off

goto :start

:getVersion
set "sharedAssemblyInfoPath=..\Core\SharedAssemblyInfo.cs"

if not exist "%sharedAssemblyInfoPath%" (
    echo Error: SharedAssemblyInfo.cs not found.
    exit /b
)

for /f "tokens=2 delims=()" %%a in ('findstr /i "AssemblyInformationalVersion" "%sharedAssemblyInfoPath%"') do (
    set "assemblyVersion=%%~a"
)
exit /b

:start

set "sevenZipPath=C:\Program Files\7-Zip\7z.exe"
if not exist "%sevenZipPath%" (
    echo Error: 7-Zip not found at specified path.
    exit /b 1
)

set "releaseFolder=..\UI\bin\Release\net8.0-windows"
set "debugFolder=..\UI\bin\Debug\net8.0-windows"

if exist "%releaseFolder%" (
    set "folderToZip=%releaseFolder%"
    set "buildProfile=Release"
) else if exist "%debugFolder%" (
    set "folderToZip=%debugFolder%"
    set "buildProfile=Debug"
) else (
    echo Error: No 'Release' neither 'Debug' folder found!
    exit /b 2
)

call :getVersion
set "outputZipFile=..\SysmacStudioParameterEditorUserSelectionFileMaker_v%assemblyVersion%.zip"

if exist %outputZipFile% (
    del %outputZipFile%
)

"%sevenZipPath%" a -r -tzip "%outputZipFile%" "%folderToZip%\*" >nul 2>nul
"%sevenZipPath%" a -r -tzip "%outputZipFile%" "..\CHANGELOG.md" >nul 2>nul
"%sevenZipPath%" a -r -tzip "%outputZipFile%" "..\README.md" >nul 2>nul

echo %outputZipFile% ready (%buildProfile%).
