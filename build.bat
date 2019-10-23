@echo off

set config=Release
set platform=AnyCpu
:: Use current directory
set cwd=%CD%
:: Switch disk and folder
cd /D ^"%cwd%^"
:: Use current dirrectory for output directory
set outputdir=%cwd%\bin
set commonflags=/p:Configuration=%config% /p:Platform=%platform% /p:DebugSymbols=false /p:DebugType=None

set fdir=%WINDIR%\Microsoft.NET\Framework
set msbuild=%fdir%\v4.0.30319\msbuild.exe

:build
echo ---------------------------------------------------------------------
echo Compile started...
::Add quotes for folderw with spaces
%msbuild% "src\Fleck2.csproj" %commonflags% /tv:2.0 /p:TargetFrameworkVersion=v2.0 /p:OutputPath="%outputdir%\NET20"
%msbuild% "src\Fleck2.csproj" %commonflags% /tv:3.5 /p:TargetFrameworkVersion=v3.5 /p:OutputPath="%outputdir%\NET35"
%msbuild% "src\Fleck2.csproj" %commonflags% /tv:4.0 /p:TargetFrameworkVersion=v4.0 /p:OutputPath="%outputdir%\NET40"
:: After compile for .NET Framework 4.0 this will working for net40
%msbuild% "demo\Fleck2.Demo.csproj" /property:Configuration=Release /p:IntermediateOutputPath="../demo/bin/Release/"
%msbuild% "src\Fleck2.csproj" %commonflags% /tv:4.0 /p:TargetFrameworkVersion=v4.5 /p:OutputPath="%outputdir%\NET45"
%msbuild% "src\Fleck2.csproj" %commonflags% /tv:4.0 /p:TargetFrameworkVersion=v4.5 /p:OutputPath="%outputdir%\NET45"

:done
echo.
echo ---------------------------------------------------------------------
echo Compile finished....
goto exit

:exit
::Don't exit, to be able to read errors.
pause