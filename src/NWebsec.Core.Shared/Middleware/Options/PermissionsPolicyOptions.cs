// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.Core.Common.HttpHeaders;
using NWebsec.Core.Common.HttpHeaders.Configuration;

namespace NWebsec.Core.Common.Middleware.Options
{
    public class PermissionsPolicyOptions : IPermissionsPolicyConfiguration, IFluentPermissionsPolicyOptions
    {
        public bool Enabled { get; set; } = true;
        public IPermissionsPolicyPermissionConfiguration AccelerometerPermission { get; set; } = new PermissionsPolicyPermission();
        public IPermissionsPolicyPermissionConfiguration AmbientLightSensorPermission { get; set; } = new PermissionsPolicyPermission();
        public IPermissionsPolicyPermissionConfiguration AutoplayPermission { get; set; } = new PermissionsPolicyPermission();
        public IPermissionsPolicyPermissionConfiguration BatteryPermission { get; set; } = new PermissionsPolicyPermission();
        public IPermissionsPolicyPermissionConfiguration CameraPermission { get; set; } = new PermissionsPolicyPermission();
        public IPermissionsPolicyPermissionConfiguration CrossOriginIsolatedPermission { get; set; } = new PermissionsPolicyPermission();
        public IPermissionsPolicyPermissionConfiguration DisplayCapturePermission { get; set; } = new PermissionsPolicyPermission();
        public IPermissionsPolicyPermissionConfiguration DocumentDomainPermission { get; set; } = new PermissionsPolicyPermission();
        public IPermissionsPolicyPermissionConfiguration EncryptedMediaPermission { get; set; } = new PermissionsPolicyPermission();
        public IPermissionsPolicyPermissionConfiguration ExecutionWhileNotRenderedPermission{ get; set; } = new PermissionsPolicyPermission();
        public IPermissionsPolicyPermissionConfiguration ExecutionWhileOutOfViewportPermission{ get; set; } = new PermissionsPolicyPermission();
        public IPermissionsPolicyPermissionConfiguration FullscreenPermission { get; set; } = new PermissionsPolicyPermission();
        public IPermissionsPolicyPermissionConfiguration GeolocationPermission { get; set; } = new PermissionsPolicyPermission();
        public IPermissionsPolicyPermissionConfiguration GyroscopePermission { get; set; } = new PermissionsPolicyPermission();
        public IPermissionsPolicyPermissionConfiguration HidPermission { get; set; } = new PermissionsPolicyPermission();
        public IPermissionsPolicyPermissionConfiguration IdleDetectionPermission { get; set; } = new PermissionsPolicyPermission();
        public IPermissionsPolicyPermissionConfiguration MagnetometerPermission { get; set; } = new PermissionsPolicyPermission();
        public IPermissionsPolicyPermissionConfiguration MicrophonePermission { get; set; } = new PermissionsPolicyPermission();
        public IPermissionsPolicyPermissionConfiguration MidiPermission { get; set; } = new PermissionsPolicyPermission();
        public IPermissionsPolicyPermissionConfiguration NavigationOverridePermission { get; set; } = new PermissionsPolicyPermission();
        public IPermissionsPolicyPermissionConfiguration PaymentPermission { get; set; } = new PermissionsPolicyPermission();
        public IPermissionsPolicyPermissionConfiguration PictureInPicturePermission { get; set; } = new PermissionsPolicyPermission();
        public IPermissionsPolicyPermissionConfiguration PublickeyCredentialsGetPermission {get; set; } = new PermissionsPolicyPermission();
        public IPermissionsPolicyPermissionConfiguration ScreenWakeLockPermission { get; set; } = new PermissionsPolicyPermission();
        public IPermissionsPolicyPermissionConfiguration SerialPermission { get; set; } = new PermissionsPolicyPermission();
        public IPermissionsPolicyPermissionConfiguration SyncXhrPermission { get; set; } = new PermissionsPolicyPermission();
        public IPermissionsPolicyPermissionConfiguration UsbPermission { get; set; } = new PermissionsPolicyPermission();
        public IPermissionsPolicyPermissionConfiguration WebSharePermission { get; set; } = new PermissionsPolicyPermission(); 
        public IPermissionsPolicyPermissionConfiguration XrSpatialTrackingPermission { get; set; } = new PermissionsPolicyPermission();
        public IFluentPermissionsPolicyOptions AccelerometerSources(Action<IPermissionsPolicyPermissionConfiguration> configurer)
        {
            configurer(AccelerometerPermission);
            return this;
        }

        public IFluentPermissionsPolicyOptions AmbientLightSensorSources(Action<IPermissionsPolicyPermissionConfiguration> configurer)
        {
            configurer(AmbientLightSensorPermission);
            return this;
        }

        public IFluentPermissionsPolicyOptions AutoplaySources(Action<IPermissionsPolicyPermissionConfiguration> configurer)
        {
            configurer(AutoplayPermission);
            return this;
        }

        public IFluentPermissionsPolicyOptions BatterySources(Action<IPermissionsPolicyPermissionConfiguration> configurer)
        {
            configurer(BatteryPermission);
            return this;
        }

        public IFluentPermissionsPolicyOptions CameraSources(Action<IPermissionsPolicyPermissionConfiguration> configurer)
        {
            configurer(CameraPermission);
            return this;
        }

        public IFluentPermissionsPolicyOptions CrossOriginIsolatedSources(Action<IPermissionsPolicyPermissionConfiguration> configurer)
        {
            configurer(CrossOriginIsolatedPermission);
            return this;
        }

        public IFluentPermissionsPolicyOptions DisplayCaptureSources(Action<IPermissionsPolicyPermissionConfiguration> configurer)
        {
            configurer(DisplayCapturePermission);
            return this;
        }

        public IFluentPermissionsPolicyOptions DocumentDomainSources(Action<IPermissionsPolicyPermissionConfiguration> configurer)
        {
            configurer(DocumentDomainPermission);
            return this;
        }

        public IFluentPermissionsPolicyOptions EncryptedMediaSources(Action<IPermissionsPolicyPermissionConfiguration> configurer)
        {
            configurer(EncryptedMediaPermission);
            return this;
        }

        public IFluentPermissionsPolicyOptions ExecutionWhileNotRenderedSources(Action<IPermissionsPolicyPermissionConfiguration> configurer)
        {
            configurer(ExecutionWhileNotRenderedPermission);
            return this;
        }

        public IFluentPermissionsPolicyOptions ExecutionWhileOutOfViewportSources(Action<IPermissionsPolicyPermissionConfiguration> configurer)
        {
            configurer(ExecutionWhileOutOfViewportPermission);
            return this;
        }

        public IFluentPermissionsPolicyOptions FullscreenSources(Action<IPermissionsPolicyPermissionConfiguration> configurer)
        {
            configurer(FullscreenPermission);
            return this;
        }

        public IFluentPermissionsPolicyOptions GeolocationSources(Action<IPermissionsPolicyPermissionConfiguration> configurer)
        {
            configurer(GeolocationPermission);
            return this;
        }

        public IFluentPermissionsPolicyOptions GyroscopeSources(Action<IPermissionsPolicyPermissionConfiguration> configurer)
        {
            configurer(GyroscopePermission);
            return this;
        }

        public IFluentPermissionsPolicyOptions HidSources(Action<IPermissionsPolicyPermissionConfiguration> configurer)
        {
            configurer(HidPermission);
            return this;
        }

        public IFluentPermissionsPolicyOptions IdleDetectionSources(Action<IPermissionsPolicyPermissionConfiguration> configurer)
        {
            configurer(IdleDetectionPermission);
            return this;
        }

        public IFluentPermissionsPolicyOptions MagnetometerSources(Action<IPermissionsPolicyPermissionConfiguration> configurer)
        {
            configurer(MagnetometerPermission);
            return this;
        }

        public IFluentPermissionsPolicyOptions MicrophoneSources(Action<IPermissionsPolicyPermissionConfiguration> configurer)
        {
            configurer(MicrophonePermission);
            return this;
        }

        public IFluentPermissionsPolicyOptions MidiSources(Action<IPermissionsPolicyPermissionConfiguration> configurer)
        {
            configurer(MidiPermission);
            return this;
        }

        public IFluentPermissionsPolicyOptions NavigationOverrideSources(Action<IPermissionsPolicyPermissionConfiguration> configurer)
        {
            configurer(NavigationOverridePermission);
            return this;
        }

        public IFluentPermissionsPolicyOptions PaymentSources(Action<IPermissionsPolicyPermissionConfiguration> configurer)
        {
            configurer(PaymentPermission);
            return this;
        }

        public IFluentPermissionsPolicyOptions PictureInPictureSources(Action<IPermissionsPolicyPermissionConfiguration> configurer)
        {
            configurer(PictureInPicturePermission);
            return this;
        }

        public IFluentPermissionsPolicyOptions PublickeyCredentialsGetSources(Action<IPermissionsPolicyPermissionConfiguration> configurer)
        {
            configurer(PublickeyCredentialsGetPermission);
            return this;
        }

        public IFluentPermissionsPolicyOptions ScreenWakeLockSources(Action<IPermissionsPolicyPermissionConfiguration> configurer)
        {
            configurer(ScreenWakeLockPermission);
            return this;
        }

        public IFluentPermissionsPolicyOptions SerialSources(Action<IPermissionsPolicyPermissionConfiguration> configurer)
        {
            configurer(SerialPermission);
            return this;
        }

        public IFluentPermissionsPolicyOptions SyncXhrSources(Action<IPermissionsPolicyPermissionConfiguration> configurer)
        {
            configurer(SyncXhrPermission);
            return this;
        }

        public IFluentPermissionsPolicyOptions UsbSources(Action<IPermissionsPolicyPermissionConfiguration> configurer)
        {
            configurer(UsbPermission);
            return this;
        }

        public IFluentPermissionsPolicyOptions WebShareSources(Action<IPermissionsPolicyPermissionConfiguration> configurer)
        {
            configurer(WebSharePermission);
            return this;
        }

        public IFluentPermissionsPolicyOptions XrSpatialTrackingSources(Action<IPermissionsPolicyPermissionConfiguration> configurer)
        {
            configurer(XrSpatialTrackingPermission);
            return this;
        }
    }
}
