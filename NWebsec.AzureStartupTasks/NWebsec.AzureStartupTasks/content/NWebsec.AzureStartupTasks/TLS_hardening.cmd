REM Copyright (c) André N. Klingsheim. See https://nwebsec.codeplex.com/license for license information.

IF "%NWebsecInComputeEmulator%" == "false" (
    pushd %~dp0
    PowerShell -ExecutionPolicy Unrestricted scripts\TLS_hardening.ps1 -AllowReboot 1 >> "%TEMP%\NWebsec.AzureStartupTasksLog.txt" 2>&1
	popd
)

EXIT /B %errorlevel%
