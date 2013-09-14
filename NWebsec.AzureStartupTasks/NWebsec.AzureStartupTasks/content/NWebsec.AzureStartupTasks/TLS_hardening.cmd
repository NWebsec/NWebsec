REM Copyright (c) Andr� N. Klingsheim. See https://nwebsec.codeplex.com/license for license information.

IF "%NWebsecInComputeEmulator%" == "false" (
    pushd %~dp0
    PowerShell -Version 2.0 -ExecutionPolicy Unrestricted .\TLS_hardening.ps1 -AllowReboot 1 >> "%TEMP%\NWebsec.AzureStartupTasksLog.txt" 2>&1
	popd
)

EXIT /B 0