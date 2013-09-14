@pushd NWebsec.AzureStartupTasks

@rd /s /q d:\nuget\NWebsec.AzureStartupTasks
@xcopy /s /y content d:\nuget\NWebsec.AzureStartupTasks\content\
@xcopy /s /y tools d:\nuget\NWebsec.AzureStartupTasks\tools\
@xcopy /f /y *.nuspec d:\nuget\NWebsec.AzureStartupTasks\
@popd

@echo "Now go to d:\nuget\NWebsec.AzureStartupTasks, update nuspec version number, and publish! :)"

