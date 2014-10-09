
@pushd NWebsec.SessionSecurity
@rd /s /q d:\nuget\build\nwebsecsession
msbuild NWebsec.SessionSecurity.csproj /p:Configuration=Release /p:TargetFrameworkVersion=v3.5 /p:OutputPath="d:\nuget\build\nwebsecsession\lib\35" /t:Rebuild /v:q /p:SignAssembly=true /p:AssemblyOriginatorKeyFile=C:\NWebsecKey\NWebsec.pfx
msbuild NWebsec.SessionSecurity.csproj /p:Configuration=Release /p:TargetFrameworkVersion=v4.0 /p:OutputPath="d:\nuget\build\nwebsecsession\lib\40" /t:Rebuild /v:q /p:SignAssembly=true /p:AssemblyOriginatorKeyFile=C:\NWebsecKey\NWebsec.pfx
msbuild NWebsec.SessionSecurity.csproj /p:Configuration=Release /p:TargetFrameworkVersion=v4.5 /p:OutputPath="d:\nuget\build\nwebsecsession\lib\45" /t:Rebuild /v:q /p:SignAssembly=true /p:AssemblyOriginatorKeyFile=C:\NWebsecKey\NWebsec.pfx

@rd /s /q d:\nuget\buildnuget\nwebsec.sessionsecurity
@xcopy /f /y d:\nuget\build\nwebsecsession\lib\35\NWebsec.SessionSecurity.dll d:\nuget\buildnuget\nwebsec.sessionsecurity\lib\35\
@xcopy /f /y d:\nuget\build\nwebsecsession\lib\40\NWebsec.SessionSecurity.dll d:\nuget\buildnuget\nwebsec.sessionsecurity\lib\40\
@xcopy /f /y d:\nuget\build\nwebsecsession\lib\45\NWebsec.SessionSecurity.dll d:\nuget\buildnuget\nwebsec.sessionsecurity\lib\45\
@xcopy /f /y NWebsec.SessionSecurity.nuspec d:\nuget\buildnuget\nwebsec.sessionsecurity\
@xcopy /f /y web.config.transform d:\nuget\buildnuget\nwebsec.sessionsecurity\content\
@xcopy /f /y /s d:\nuget\build\nwebsecsession\lib\40\ConfigurationSchema d:\nuget\buildnuget\nwebsec.sessionsecurity\content\NWebsecConfig\
@popd

@pushd d:\nuget\buildnuget\nwebsec.sessionsecurity
nuget.exe pack
@xcopy /f /y *.nupkg D:\nuget\Release\nwebsec.sessionsecurity\packages\
@xcopy /f /y *.nupkg D:\nuget\feed\
@popd

@echo "Now go to d:\nuget\nwebsec\, update nuspec version number, and publish! :)"

