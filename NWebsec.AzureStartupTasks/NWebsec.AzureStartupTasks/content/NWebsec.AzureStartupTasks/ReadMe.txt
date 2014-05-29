You need to add the startup tasks by hand to your ServiceDefinition.cfg.

*** TLS hardening ***
The TLS hardening script will update the relevant Schannel registry settings to
configure enabled TLS protocol versions and cipher suites according to the
latest recommendations on ssllabs.com. Refer to the NWebsec project website for
documentation on which cipher suites are enabled for each version of this
script (or have a look in the ps1 script). Changes to registry requires a
reboot to take effect, this is handled by the script. The script is clever
enough to only reboot after changes are made to the registry, avoiding
unnecessary reboots of the role instances.

You'll find a log file on you Azure role in:
%TEMP%\NWebsec.AzureStartupTasksLog.txt

Here's the required configuration for the ServiceDefinition.csdef file. Note
the environment variable which prevents the script from running when the
application is running in the Azure emulator. You probably forgot this if it
updates your registry and reboots your machine. :)

Note! The startup scripts require osFamily="2" or newer - so please check your
ServiceConfiguration.Cloud.cscfg.

Note also that the AES-GCM ciphers introduced with "Windows Server 2012 R2 Update" are available from guest OS version 4.7 (released May 2. 2014).

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