// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.Common.HttpHeaders.Configuration;

namespace NWebsec.Core.Common.HttpHeaders
{
    public class PermissionsPolicyConfiguration : IPermissionsPolicyConfiguration
    {
        public PermissionsPolicyConfiguration()
        {
            AccelerometerPermission = new PermissionsPolicyPermissionConfiguration();
            AmbientLightSensorPermission = new PermissionsPolicyPermissionConfiguration();
            AutoplayPermission = new PermissionsPolicyPermissionConfiguration();
            BatteryPermission = new PermissionsPolicyPermissionConfiguration();
            CameraPermission = new PermissionsPolicyPermissionConfiguration();
            CrossOriginIsolatedPermission = new PermissionsPolicyPermissionConfiguration();
            DisplayCapturePermission = new PermissionsPolicyPermissionConfiguration();
            DocumentDomainPermission = new PermissionsPolicyPermissionConfiguration();
            EncryptedMediaPermission = new PermissionsPolicyPermissionConfiguration();
            ExecutionWhileNotRenderedPermission = new PermissionsPolicyPermissionConfiguration();
            ExecutionWhileOutOfViewportPermission = new PermissionsPolicyPermissionConfiguration();
            FullscreenPermission = new PermissionsPolicyPermissionConfiguration();
            GeolocationPermission = new PermissionsPolicyPermissionConfiguration();
            GyroscopePermission = new PermissionsPolicyPermissionConfiguration();
            HidPermission = new PermissionsPolicyPermissionConfiguration();
            IdleDetectionPermission = new PermissionsPolicyPermissionConfiguration();
            MagnetometerPermission = new PermissionsPolicyPermissionConfiguration();
            MicrophonePermission = new PermissionsPolicyPermissionConfiguration();
            MidiPermission = new PermissionsPolicyPermissionConfiguration();
            NavigationOverridePermission = new PermissionsPolicyPermissionConfiguration();
            PaymentPermission = new PermissionsPolicyPermissionConfiguration();
            PictureInPicturePermission = new PermissionsPolicyPermissionConfiguration();
            PublickeyCredentialsGetPermission = new PermissionsPolicyPermissionConfiguration();
            ScreenWakeLockPermission = new PermissionsPolicyPermissionConfiguration();
            SerialPermission = new PermissionsPolicyPermissionConfiguration();
            SyncXhrPermission = new PermissionsPolicyPermissionConfiguration();
            UsbPermission = new PermissionsPolicyPermissionConfiguration();
            WebSharePermission = new PermissionsPolicyPermissionConfiguration();
            XrSpatialTrackingPermission = new PermissionsPolicyPermissionConfiguration();
        }

        public bool Enabled { get; set; }
        public IPermissionsPolicyPermissionConfiguration AccelerometerPermission { get; set; }
        public IPermissionsPolicyPermissionConfiguration AmbientLightSensorPermission { get; set; }
        public IPermissionsPolicyPermissionConfiguration AutoplayPermission { get; set; }
        public IPermissionsPolicyPermissionConfiguration BatteryPermission { get; set; }
        public IPermissionsPolicyPermissionConfiguration CameraPermission { get; set; }
        public IPermissionsPolicyPermissionConfiguration CrossOriginIsolatedPermission { get; set; }
        public IPermissionsPolicyPermissionConfiguration DisplayCapturePermission { get; set; }
        public IPermissionsPolicyPermissionConfiguration DocumentDomainPermission { get; set; }
        public IPermissionsPolicyPermissionConfiguration EncryptedMediaPermission { get; set; }
        public IPermissionsPolicyPermissionConfiguration ExecutionWhileNotRenderedPermission { get; set; }
        public IPermissionsPolicyPermissionConfiguration ExecutionWhileOutOfViewportPermission{ get; set; }
        public IPermissionsPolicyPermissionConfiguration FullscreenPermission { get; set; }
        public IPermissionsPolicyPermissionConfiguration GeolocationPermission { get; set; }
        public IPermissionsPolicyPermissionConfiguration GyroscopePermission { get; set; }
        public IPermissionsPolicyPermissionConfiguration HidPermission { get; set; }
        public IPermissionsPolicyPermissionConfiguration IdleDetectionPermission { get; set; }
        public IPermissionsPolicyPermissionConfiguration MagnetometerPermission { get; set; }
        public IPermissionsPolicyPermissionConfiguration MicrophonePermission { get; set; }
        public IPermissionsPolicyPermissionConfiguration MidiPermission { get; set; }
        public IPermissionsPolicyPermissionConfiguration NavigationOverridePermission { get; set; }
        public IPermissionsPolicyPermissionConfiguration PaymentPermission { get; set; }
        public IPermissionsPolicyPermissionConfiguration PictureInPicturePermission { get; set; }
        public IPermissionsPolicyPermissionConfiguration PublickeyCredentialsGetPermission {get; set;}
        public IPermissionsPolicyPermissionConfiguration ScreenWakeLockPermission { get; set; }
        public IPermissionsPolicyPermissionConfiguration SerialPermission { get; set; }
        public IPermissionsPolicyPermissionConfiguration SyncXhrPermission { get; set; }
        public IPermissionsPolicyPermissionConfiguration UsbPermission { get; set; }
        public IPermissionsPolicyPermissionConfiguration WebSharePermission { get; set; }
        public IPermissionsPolicyPermissionConfiguration XrSpatialTrackingPermission { get; set; }
    }
}
