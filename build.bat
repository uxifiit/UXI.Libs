
set config=%1
if "%config%" == "" (
   set config=Release
)

set version=
if not "%PackageVersion%" == "" (
   set version=-Version %PackageVersion%
)
REM Build
"%programfiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe" UXI-Libs.sln /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false

REM Package
mkdir Build
REM call "%nuget%" pack "src\UXI.App\UXI.App.csproj" -symbols -o Build -p Configuration=%config%

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
   UXI.SystemApi
   UXI.ZIP
) do call "%nuget%" pack ".nuget\%%p.nuspec" -symbols -o Build -p Configuration=%config% 

