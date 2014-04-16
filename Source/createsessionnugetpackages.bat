
@pushd NWebsec.SessionSecurity
msbuild NWebsec.SessionSecurity.csproj /p:Configuration=Release /p:TargetFrameworkVersion=v3.5 /p:OutputPath="d:\nuget\nwebsecsessionstage\lib\35" /t:Rebuild /v:q /p:SignAssembly=true /p:AssemblyOriginatorKeyFile=C:\NWebsecKey\NWebsec.pfx
msbuild NWebsec.SessionSecurity.csproj /p:Configuration=Release /p:TargetFrameworkVersion=v4.0 /p:OutputPath="d:\nuget\nwebsecsessionstage\lib\40" /t:Rebuild /v:q /p:SignAssembly=true /p:AssemblyOriginatorKeyFile=C:\NWebsecKey\NWebsec.pfx
msbuild NWebsec.SessionSecurity.csproj /p:Configuration=Release /p:TargetFrameworkVersion=v4.5 /p:OutputPath="d:\nuget\nwebsecsessionstage\lib\45" /t:Rebuild /v:q /p:SignAssembly=true /p:AssemblyOriginatorKeyFile=C:\NWebsecKey\NWebsec.pfx

@rd /s /q d:\nuget\nwebsec.sessionsecurity
@xcopy /f /y d:\nuget\nwebsecsessionstage\lib\35\NWebsec.SessionSecurity.dll d:\nuget\nwebsec.sessionsecurity\lib\35\
@xcopy /f /y d:\nuget\nwebsecsessionstage\lib\40\NWebsec.SessionSecurity.dll d:\nuget\nwebsec.sessionsecurity\lib\40\
@xcopy /f /y d:\nuget\nwebsecsessionstage\lib\45\NWebsec.SessionSecurity.dll d:\nuget\nwebsec.sessionsecurity\lib\45\
@xcopy /f /y NWebsec.SessionSecurity.nuspec d:\nuget\nwebsec.sessionsecurity\
@xcopy /f /y web.config.transform d:\nuget\nwebsec.sessionsecurity\content\
@xcopy /f /y /s d:\nuget\nwebsecsessionstage\lib\40\ConfigurationSchema d:\nuget\nwebsec.sessionsecurity\content\NWebsecConfig\
@popd

@echo "Now go to d:\nuget\nwebsec\, update nuspec version number, and publish! :)"

