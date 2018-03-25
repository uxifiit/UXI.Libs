
set config=%1
if "%config%" == "" (
   set config=Release
)

set version=
if not "%PackageVersion%" == "" (
   set version=-Version %PackageVersion%
)

set projects=UXI.App
set projects=%projects%;UXI.Common
set projects=%projects%;UXI.Common.UI
set projects=%projects%;UXI.Common.Web
set projects=%projects%;UXI.Common.WebApi
set projects=%projects%;UXI.Configuration
set projects=%projects%;UXI.Configuration.IniStorage
set projects=%projects%;UXI.CQRS
set projects=%projects%;UXI.CQRS.EntityFramework
set projects=%projects%;UXI.IO
set projects=%projects%;UXI.IO.NTFS
set projects=%projects%;UXI.OwinServer
set projects=%projects%;UXI.SystemApi
set projects=%projects%;UXI.ZIP

for %%p in (%projects%) do (
	if exist "src\%%p\packages.config" (
		call "%nuget%" restore "src\%%p\packages.config" -OutputDirectory %cd%\packages -NonInteractive
	)
)

REM Build
"%programfiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe" UXI-Libs.sln /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false

REM Package
mkdir Build
REM call "%nuget%" pack "src\UXI.App\UXI.App.csproj" -symbols -o Build -p Configuration=%config%


for %%p in (%projects%) do (
	call "%nuget%" pack ".nuget\%%p.nuspec" -symbols -o Build -p Configuration=%config% 
)
