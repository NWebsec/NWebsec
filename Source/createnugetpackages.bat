@pushd NWebsec

msbuild NWebsec.csproj /tv:3.5 /p:Configuration=Release /p:TargetFrameworkVersion=v3.5 /p:OutputPath="d:\nuget\nwebsecstage\lib\35" /t:Rebuild /v:q /p:DocumentationFile="NWebsec.xml" /p:NoWarn=1591 /p:SignAssembly=true /p:AssemblyOriginatorKeyFile=D:\NWebsecKey\NWebsec.pfx
msbuild NWebsec.csproj /tv:4.0 /p:Configuration=Release /p:TargetFrameworkVersion=v4.0 /p:OutputPath="d:\nuget\nwebsecstage\lib\40" /t:Rebuild /v:q /p:DocumentationFile="NWebsec.xml" /p:NoWarn=1591 /p:SignAssembly=true /p:AssemblyOriginatorKeyFile=D:\NWebsecKey\NWebsec.pfx

@rd /s /q d:\nuget\nwebsec
@xcopy /f /y d:\nuget\nwebsecstage\lib\35\NWebsec.dll d:\nuget\nwebsec\lib\35\
@xcopy /f /y d:\nuget\nwebsecstage\lib\35\NWebsec.xml d:\nuget\nwebsec\lib\35\
@xcopy /f /y d:\nuget\nwebsecstage\lib\40\NWebsec.dll d:\nuget\nwebsec\lib\40\
@xcopy /f /y d:\nuget\nwebsecstage\lib\40\NWebsec.xml d:\nuget\nwebsec\lib\40\
@xcopy /f /y NWebsec.nuspec d:\nuget\nwebsec\
@xcopy /f /y web.config.transform d:\nuget\nwebsec\content\
@xcopy /f /y /s d:\nuget\nwebsecstage\lib\40\ConfigurationSchema d:\nuget\nwebsec\content\NWebsecConfig\
@popd
@pushd NWebsec.Mvc

msbuild NWebsec.Mvc.csproj /tv:4.0 /p:Configuration=Release /p:TargetFrameworkVersion=v4.0 /p:OutputPath="d:\nuget\nwebsec.mvcstage\lib\40" /t:Rebuild /v:m /p:DocumentationFile="NWebsec.Mvc.xml" /p:NoWarn=1591 /p:SignAssembly=true /p:AssemblyOriginatorKeyFile=D:\NWebsecKey\NWebsec.pfx

@rd /s /q d:\nuget\nwebsec.mvc
@xcopy /f /y d:\nuget\nwebsec.mvcstage\lib\40\NWebsec.Mvc.dll d:\nuget\nwebsec.mvc\lib\40\
@xcopy /f /y d:\nuget\nwebsec.mvcstage\lib\40\NWebsec.Mvc.xml d:\nuget\nwebsec.mvc\lib\40\

@xcopy /f /y NWebsec.Mvc.nuspec d:\nuget\nwebsec.Mvc\
@popd
@pushd NWebsec.SessionSecurity
msbuild NWebsec.SessionSecurity.csproj /tv:4.0 /p:Configuration=Release /p:TargetFrameworkVersion=v3.5 /p:OutputPath="d:\nuget\nwebsecsessionstage\lib\35" /t:Rebuild /v:q /p:SignAssembly=true /p:AssemblyOriginatorKeyFile=D:\NWebsecKey\NWebsec.pfx
msbuild NWebsec.SessionSecurity.csproj /tv:4.0 /p:Configuration=Release /p:TargetFrameworkVersion=v4.0 /p:OutputPath="d:\nuget\nwebsecsessionstage\lib\40" /t:Rebuild /v:q /p:SignAssembly=true /p:AssemblyOriginatorKeyFile=D:\NWebsecKey\NWebsec.pfx
msbuild NWebsec.SessionSecurity.csproj /tv:4.0 /p:Configuration=Release /p:TargetFrameworkVersion=v4.5 /p:OutputPath="d:\nuget\nwebsecsessionstage\lib\45" /t:Rebuild /v:q /p:SignAssembly=true /p:AssemblyOriginatorKeyFile=D:\NWebsecKey\NWebsec.pfx

@rd /s /q d:\nuget\nwebsec.sessionsecurity
@xcopy /f /y d:\nuget\nwebsecsessionstage\lib\35\NWebsec.SessionSecurity.dll d:\nuget\nwebsec.sessionsecurity\lib\35\
@xcopy /f /y d:\nuget\nwebsecsessionstage\lib\40\NWebsec.SessionSecurity.dll d:\nuget\nwebsec.sessionsecurity\lib\40\
@xcopy /f /y d:\nuget\nwebsecsessionstage\lib\45\NWebsec.SessionSecurity.dll d:\nuget\nwebsec.sessionsecurity\lib\45\
@xcopy /f /y NWebsec.SessionSecurity.nuspec d:\nuget\nwebsec.sessionsecurity\
@xcopy /f /y web.config.transform d:\nuget\nwebsec.sessionsecurity\content\
@xcopy /f /y /s d:\nuget\nwebsecsessionstage\lib\40\ConfigurationSchema d:\nuget\nwebsec.sessionsecurity\content\NWebsecConfig\
@popd

@echo "Now go to d:\nuget\nwebsec\, update nuspec version number, and publish! :)"

