@echo off

set config=Release
set target=bin
set cwd=%CD%
set commonflags=/p:DebugSymbols=false /p:DebugType=None

if not exist %fdir% (
    set fdir=%WINDIR%\Microsoft.NET\Framework
)

set msbuild=%fdir%\v4.0.30319\msbuild.exe

:build
echo ---------------------------------------------------------------------
echo Compile started...
%msbuild% src\Fleck2.csproj /p:Configuration=%config%;Platform=AnyCpu %commonflags% /tv:2.0 /p:TargetFrameworkVersion=v2.0 /p:OutputPath="%cwd%\bin\NET20"
%msbuild% src\Fleck2.csproj /p:Configuration=%config%;Platform=AnyCpu %commonflags% /tv:3.5 /p:TargetFrameworkVersion=v3.5 /p:OutputPath="%cwd%\bin\NET35"
%msbuild% src\Fleck2.csproj /p:Configuration=%config%;Platform=AnyCpu %commonflags% /tv:4.0 /p:TargetFrameworkVersion=v4.0 /p:OutputPath="%cwd%\bin\NET40"
%msbuild% src\Fleck2.csproj /p:Configuration=%config%;Platform=AnyCpu %commonflags% /tv:4.0 /p:TargetFrameworkVersion=v4.5 /p:OutputPath="%cwd%\bin\NET45"

:done
echo.
echo ---------------------------------------------------------------------
echo Compile finished....
goto exit

:exit