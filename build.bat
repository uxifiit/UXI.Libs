echo off

set config=%1
if "%config%" == "" (
   set config=Release
)

set version=
if not "%PackageVersion%" == "" (
   set version=-Version %PackageVersion%
)

REM Restore packages
call "%nuget%" restore UXI.Libs.sln -NonInteractive

REM Build
"%programfiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe" UXI.Libs.sln /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false

REM Package
mkdir Build

for %%p in (
UXI.App
UXI.Common
UXI.Common.UI
UXI.Common.Web
UXI.Common.WebApi
UXI.Configuration
UXI.Configuration.IniStorage
UXI.CQRS
UXI.CQRS.EntityFramework
UXI.IO
UXI.IO.NTFS
UXI.OwinServer
UXI.Serialization
UXI.SystemApi
UXI.ZIP
) do (
	call "%nuget%" pack ".nuget\%%p.nuspec" -Symbols -OutputDirectory build -Properties Configuration=%config% 
)
