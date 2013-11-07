@echo off

set config=Release
set platform=AnyCpu
set outputdir=%cwd%\bin
set cwd=%CD%
set commonflags=/p:Configuration=%config% /p:Platform=%platform% /p:DebugSymbols=false 

set fdir=%WINDIR%\Microsoft.NET\Framework
set msbuild=%fdir%\v4.0.30319\msbuild.exe

:build
echo ---------------------------------------------------------------------
echo Compile started...
rem %msbuild% src\Fleck2.csproj %commonflags% /tv:2.0 /p:TargetFrameworkVersion=v2.0 /p:OutputPath="%outputdir%\NET20"
%msbuild% src\Fleck2.csproj %commonflags% /p:DebugType=None /tv:3.5 /p:TargetFrameworkVersion=v3.5 /p:OutputPath="%outputdir%\NET35"
rem %msbuild% src\Fleck2.csproj %commonflags% /p:DebugType=None /tv:4.0 /p:TargetFrameworkVersion=v4.0 /p:OutputPath="%outputdir%\NET40"
rem %msbuild% src\Fleck2.csproj %commonflags% /p:DebugType=None /tv:4.0 /p:TargetFrameworkVersion=v4.5 /p:OutputPath="%outputdir%\NET45"

:done
echo.
echo ---------------------------------------------------------------------
echo Compile finished....
goto exit

:exit