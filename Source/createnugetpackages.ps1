$ErrorActionPreference = "Stop" 

function RemoveDirIfExists($item){
	if (Test-Path $item){
	rm $item -recurse
	}
}

function FatalError{
	Write-Host -ForegroundColor Red "Fatal error. Exit code was $LASTEXITCODE. Exiting"
	exit
}

function CheckExitCode{
	if ($LASTEXITCODE -ne 0){FatalError}
}

function CheckRobocopyExitCode{
	if ($LASTEXITCODE -ne 1){FatalError}
}

# *************CORE**************
pushd NWebsec.Core

Write-Host -ForegroundColor Green "Building NWebsec.Core"
RemoveDirIfExists("d:\nuget\build\nwebsec.core")
msbuild NWebsec.Core.csproj /p:Configuration=Release /p:TargetFrameworkVersion=v3.5 /p:OutputPath="d:\nuget\build\nwebsec.core\lib\35" /t:Rebuild /v:q /p:DocumentationFile="NWebsec.Core.xml" /p:NoWarn=1591 /p:SignAssembly=true /p:AssemblyOriginatorKeyFile=C:\NWebsecKey\NWebsec.pfx
CheckExitCode
msbuild NWebsec.Core.csproj /p:Configuration=Release /p:TargetFrameworkVersion=v4.0 /p:OutputPath="d:\nuget\build\nwebsec.core\lib\40" /t:Rebuild /v:q /p:DocumentationFile="NWebsec.Core.xml" /p:NoWarn=1591 /p:SignAssembly=true /p:AssemblyOriginatorKeyFile=C:\NWebsecKey\NWebsec.pfx
CheckExitCode
msbuild NWebsec.Core.csproj /p:Configuration=Release /p:TargetFrameworkVersion=v4.5 /p:OutputPath="d:\nuget\build\nwebsec.core\lib\45" /t:Rebuild /v:q /p:DocumentationFile="NWebsec.Core.xml" /p:NoWarn=1591 /p:SignAssembly=true /p:AssemblyOriginatorKeyFile=C:\NWebsecKey\NWebsec.pfx
CheckExitCode

Write-Host -ForegroundColor Green "Copying NWebsec.Core"
RemoveDirIfExists("d:\nuget\buildnuget\nwebsec.core")
robocopy /s d:\nuget\build\nwebsec.core d:\nuget\buildnuget\nwebsec.core NWebsec.Core.dll NWebsec.Core.xml /ns /nc /np /njh /njs
CheckRobocopyExitCode
cp NWebsec.Core.nuspec d:\nuget\buildnuget\nwebsec.core
popd

RemoveDirIfExists("D:\nuget\Release\packages")
New-Item -ItemType directory -Path "D:\nuget\Release\packages"

Write-Host -ForegroundColor Green "Creating NWebsec.Core nuget package"
pushd d:\nuget\buildnuget\nwebsec.core
nuget.exe pack
CheckExitCode

Write-Host -ForegroundColor Green "Copying NWebsec.Core nuget package"
cp *.nupkg D:\nuget\Release\packages
cp *.nupkg D:\nuget\feed
popd

# *************OWIN**************
pushd NWebsec.Owin

Write-Host -ForegroundColor Green "Building NWebsec.Owin"
RemoveDirIfExists("d:\nuget\build\nwebsec.owin")
msbuild NWebsec.Owin.csproj /p:Configuration=Release /p:TargetFrameworkVersion=v4.5 /p:OutputPath="d:\nuget\build\nwebsec.owin\lib\45" /t:Rebuild /v:q /p:DocumentationFile="NWebsec.Owin.xml" /p:NoWarn=1591 /p:SignAssembly=true /p:AssemblyOriginatorKeyFile=C:\NWebsecKey\NWebsec.pfx
CheckExitCode

Write-Host -ForegroundColor Green "Copying NWebsec.Owin"
RemoveDirIfExists("d:\nuget\buildnuget\nwebsec.owin")
robocopy /s  d:\nuget\build\nwebsec.owin\ d:\nuget\buildnuget\nwebsec.owin\ Nwebsec.Owin.dll, NWebsec.Owin.xml /ns /nc /np /njh /njs
CheckRobocopyExitCode
cp NWebsec.Owin.nuspec d:\nuget\buildnuget\nwebsec.owin\
popd

Write-Host -ForegroundColor Green "Creating NWebsec.Owin nuget package"
pushd d:\nuget\buildnuget\nwebsec.owin
nuget.exe pack
CheckExitCode

Write-Host -ForegroundColor Green "Copying NWebsec.Owin nuget package"
cp *.nupkg D:\nuget\Release\packages
cp *.nupkg D:\nuget\feed
popd

# *************NWebsec**************
Write-Host -ForegroundColor Green "Building NWebsec"
pushd NWebsec
RemoveDirIfExists("d:\nuget\build\nwebsec")
msbuild NWebsec.Classic.csproj /p:Configuration=Release /p:TargetFrameworkVersion=v3.5 /p:OutputPath="d:\nuget\build\nwebsec\lib\35" /t:Rebuild /v:q /p:DocumentationFile="NWebsec.xml" /p:NoWarn=1591 /p:SignAssembly=true /p:AssemblyOriginatorKeyFile=C:\NWebsecKey\NWebsec.pfx
CheckExitCode
msbuild NWebsec.Classic.csproj /p:Configuration=Release /p:TargetFrameworkVersion=v4.0 /p:OutputPath="d:\nuget\build\nwebsec\lib\40" /t:Rebuild /v:q /p:DocumentationFile="NWebsec.xml" /p:NoWarn=1591 /p:SignAssembly=true /p:AssemblyOriginatorKeyFile=C:\NWebsecKey\NWebsec.pfx
CheckExitCode
msbuild NWebsec.Classic.csproj /p:Configuration=Release /p:TargetFrameworkVersion=v4.5 /p:OutputPath="d:\nuget\build\nwebsec\lib\45" /t:Rebuild /v:q /p:DocumentationFile="NWebsec.xml" /p:NoWarn=1591 /p:SignAssembly=true /p:AssemblyOriginatorKeyFile=C:\NWebsecKey\NWebsec.pfx
CheckExitCode

Write-Host -ForegroundColor Green "Copying NWebsec"
RemoveDirIfExists("d:\nuget\buildnuget\nwebsec")
robocopy /s d:\nuget\build\nwebsec\ d:\nuget\buildnuget\nwebsec\ NWebsec.dll NWebsec.xml /ns /nc /np /njh /njs
CheckRobocopyExitCode
robocopy d:\nuget\build\nwebsec\lib\40\ConfigurationSchema\ d:\nuget\buildnuget\nwebsec\content\NWebsecConfig\ /ns /nc /np /njh /njs
CheckRobocopyExitCode
cp NWebsec.nuspec d:\nuget\buildnuget\nwebsec
cp web.config.transform d:\nuget\buildnuget\nwebsec\content
popd

Write-Host -ForegroundColor Green "Creating NWebsec nuget package"
pushd d:\nuget\buildnuget\nwebsec
nuget.exe pack
CheckExitCode

Write-Host -ForegroundColor Green "Copying NWebsec nuget package"
cp *.nupkg D:\nuget\Release\packages
cp *.nupkg D:\nuget\feed
popd

# *************Nwebsec.Mvc**************

pushd NWebsec.Mvc
Write-Host -ForegroundColor Green "Building NWebsec.Mvc"
RemoveDirIfExists("d:\nuget\build\nwebsec.mvc")
msbuild NWebsec.Mvc.csproj /p:Configuration=Release /p:TargetFrameworkVersion=v4.0 /p:OutputPath="d:\nuget\build\nwebsec.mvc\lib\40" /t:Rebuild /v:q /p:DocumentationFile="NWebsec.Mvc.xml" /p:NoWarn=1591 /p:SignAssembly=true /p:AssemblyOriginatorKeyFile=C:\NWebsecKey\NWebsec.pfx
CheckExitCode
msbuild NWebsec.Mvc.csproj /p:Configuration=Release /p:TargetFrameworkVersion=v4.5 /p:OutputPath="d:\nuget\build\nwebsec.mvc\lib\45" /t:Rebuild /v:q /p:DocumentationFile="NWebsec.Mvc.xml" /p:NoWarn=1591 /p:SignAssembly=true /p:AssemblyOriginatorKeyFile=C:\NWebsecKey\NWebsec.pfx
CheckExitCode

Write-Host -ForegroundColor Green "Copying NWebsec.Mvc"
RemoveDirIfExists("d:\nuget\buildnuget\nwebsec.mvc")
robocopy /s d:\nuget\build\nwebsec.mvc\ d:\nuget\buildnuget\nwebsec.mvc\ Nwebsec.Mvc.dll NWebsec.Mvc.xml /ns /nc /np /njh /njs
CheckRobocopyExitCode
cp NWebsec.Mvc.nuspec d:\nuget\buildnuget\nwebsec.Mvc\
popd

Write-Host -ForegroundColor Green "Creating NWebsec.Mvc nuget package"
pushd d:\nuget\buildnuget\nwebsec.mvc
nuget.exe pack
CheckExitCode

Write-Host -ForegroundColor Green "Copying NWebsec.Mvc nuget package"
cp *.nupkg D:\nuget\Release\packages
cp *.nupkg D:\nuget\feed
popd

RemoveDirIfExists("D:\nuget\Release\Codeplex")

Write-Host -ForegroundColor Green "Copying NWebsec release files"
robocopy /s d:\nuget\buildnuget\nwebsec.core\ D:\nuget\Release\Codeplex\NWebsec\nwebsec.core\ /xf *.nuspec, *.nupkg /ns /nc /np /njh /njs
CheckRobocopyExitCode
robocopy /s d:\nuget\buildnuget\nwebsec\ D:\nuget\Release\Codeplex\NWebsec\nwebsec\ /xf *.nuspec *.nupkg /ns /nc /np /njh /njs
CheckRobocopyExitCode
robocopy /s d:\nuget\buildnuget\nwebsec.mvc\ D:\nuget\Release\Codeplex\NWebsec\nwebsec.mvc\ /xf *.nuspec *.nupkg /ns /nc /np /njh /njs
CheckRobocopyExitCode

Write-Host -ForegroundColor Green "Copying NWebsec.Owin release files"
robocopy /s d:\nuget\buildnuget\nwebsec.core D:\nuget\Release\Codeplex\NWebsec.Owin\nwebsec.core /xf *.nuspec *.nupkg /ns /nc /np /njh /njs
CheckRobocopyExitCode
robocopy /s d:\nuget\buildnuget\nwebsec.owin D:\nuget\Release\Codeplex\NWebsec.Owin\nwebsec.owin /xf *.nuspec, *.nupkg /ns /nc /np /njh /njs
CheckRobocopyExitCode

Write-Host "Renaming web.config.transform files."
Get-ChildItem -Path D:\nuget\Release\Codeplex -Filter "web.config.transform" -Recurse | Rename-Item -NewName "web.config"
#forfiles /p D:\nuget\Release\Codeplex\ /S /M "*.transform" /C "cmd /c rename @file @fname"
Write-Host -ForegroundColor Green "Build finished. Well done."
Write-Host ""

$now=Get-Date -format "dd-MM-yyyy HH:mm"
Write-Host -ForegroundColor Green $now