msbuild NWebsec.csproj /tv:3.5 /p:Configuration=Release /p:TargetFrameworkVersion=v3.5 /p:OutputPath="d:\nuget\nwebsec\lib\35" /t:Clean
msbuild NWebsec.csproj /tv:4.0 /p:Configuration=Release /p:TargetFrameworkVersion=v4.0 /p:OutputPath="d:\nuget\nwebsec\lib\40" /t:Clean

msbuild NWebsec.csproj /tv:3.5 /p:Configuration=Release /p:TargetFrameworkVersion=v3.5 /p:OutputPath="d:\nuget\nwebsec\lib\35" /t:Rebuild
msbuild NWebsec.csproj /tv:4.0 /p:Configuration=Release /p:TargetFrameworkVersion=v4.0 /p:OutputPath="d:\nuget\nwebsec\lib\40" /t:Rebuild

@cp NWebsec.nuspec d:\nuget\nwebsec\
@cp web.config.transform d:\nuget\nwebsec\content\
@echo "Now go to d:\nuget\nwebsec\, update nuspec version number, and publish! :)"