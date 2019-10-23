:: This file need to compile demo on .NET Framework 4.0
:: output files will be the folder /bin/Release
@echo off
set fdir=%WINDIR%\Microsoft.NET\Framework
set msbuild=%fdir%\v4.0.30319\msbuild.exe
%msbuild% /p:IntermediateOutputPath="bin/Release/" /property:Configuration=Release Fleck2.Demo.csproj
pause