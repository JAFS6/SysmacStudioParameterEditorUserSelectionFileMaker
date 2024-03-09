@echo off

set "buildProfileRelease=Release"
set "buildProfileDebug=Debug"

if "%1" == "" (
    rem Arguments not provided
    rem Ask the user to choose Debug or Release configuration
    choice /C DR /M "Compile in Debug or Release mode (D/R)?" /N

    rem Check the user"s choice
    if errorlevel 2 (
        set "buildProfile=%buildProfileRelease%"
    ) else (
        set "buildProfile=%buildProfileDebug%"
    )
) else (
    if "%1" neq "%buildProfileRelease%" (
        if "%1" neq "%buildProfileDebug%" (
            echo Error: Incorrect build profile "%1"
            exit /b 1
        )
    )
    set "buildProfile=%1"
)

rem Remove app bin and obj folders
rd /s /q "..\UI\bin"
rd /s /q "..\UI\obj"
rd /s /q "..\Tests\bin"
rd /s /q "..\Tests\obj"
echo Removed app bin and obj folders

rem Set the path to the .NET CLI
set DOTNET_PATH="C:\Program Files\dotnet"

rem Navigate to the directory containing the solution
cd "..\"

%DOTNET_PATH%\dotnet restore
echo NuGet packages restored

%DOTNET_PATH%\dotnet build --configuration %buildProfile%
echo %buildProfile% build complete.
