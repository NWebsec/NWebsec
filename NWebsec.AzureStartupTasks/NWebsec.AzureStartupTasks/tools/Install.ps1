param($installPath, $toolsPath, $package, $project)
$project.ProjectItems.Item("NWebsec.AzureStartupTasks").ProjectItems.Item("ReadMe.txt").Properties.Item("BuildAction").Value = 0
$project.ProjectItems.Item("NWebsec.AzureStartupTasks").ProjectItems.Item("TLS_hardening.cmd").Properties.Item("CopyToOutputDirectory").Value = 1
$project.ProjectItems.Item("NWebsec.AzureStartupTasks").ProjectItems.Item("TLS_hardening.ps1").Properties.Item("CopyToOutputDirectory").Value = 1
