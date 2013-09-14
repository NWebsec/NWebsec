To use the startup scripts you need to add the startup tasks by hand to your ServiceDefintion.cfg.

*** TLS hardening ***
The TLS hardening script will update the relevant registry settings to configure the cipher suites used by schannel. Changes to registry
requires a reboot to take effect, this is handled by the script. The script is clever enough to only reboot after changes are made to the registry,
avoiding unnecessary reboots of the role.

You'll find a log file on you Azure role in: %TEMP%\NWebsec.AzureStartupTasksLog.txt

Here's the required configuration for the ServiceDefinition.csdef file. Note the environment variable, that prevents the script from running when
the application is running in the Azure emulator. If it updates your registry and reboots your machine, you probably forgot this. :)

Note! The startup scripts requires osFamily="2" or newer - please check your ServiceConfiguration.Cloud.cscfg.

<ServiceDefinition>
  <WebRole>
    <Startup>
      <Task commandLine="NWebsec.AzureStartupTasks\TLS_hardening.cmd" executionContext="elevated" taskType="simple">
        <Environment>
          <Variable name="NWebsecInComputeEmulator">
            <RoleInstanceValue xpath="/RoleEnvironment/Deployment/@emulated" />
          </Variable>
        </Environment>
      </Task>
    </Startup>
  </WebRole>
</ServiceDefinition>