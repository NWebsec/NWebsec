// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.Common.HttpHeaders.Configuration;

namespace NWebsec.Core.Common.HttpHeaders
{
    public interface IPermissionsPolicyConfiguration
    {
        bool Enabled { get; set; }
        IPermissionsPolicyPermissionConfiguration AccelerometerPermission { get; set; }
        IPermissionsPolicyPermissionConfiguration AmbientLightSensorPermission { get; set; }
        IPermissionsPolicyPermissionConfiguration AutoplayPermission { get; set; }
        IPermissionsPolicyPermissionConfiguration BatteryPermission { get; set; }
        IPermissionsPolicyPermissionConfiguration CameraPermission { get; set; }
        IPermissionsPolicyPermissionConfiguration CrossOriginIsolatedPermission { get; set; }
        IPermissionsPolicyPermissionConfiguration DisplayCapturePermission { get; set; }
        IPermissionsPolicyPermissionConfiguration DocumentDomainPermission { get; set; }
        IPermissionsPolicyPermissionConfiguration EncryptedMediaPermission { get; set; }
        IPermissionsPolicyPermissionConfiguration ExecutionWhileNotRenderedPermission { get; set; }
        IPermissionsPolicyPermissionConfiguration ExecutionWhileOutOfViewportPermission { get; set; }
        IPermissionsPolicyPermissionConfiguration FullscreenPermission { get; set; }
        IPermissionsPolicyPermissionConfiguration GeolocationPermission { get; set; }
        IPermissionsPolicyPermissionConfiguration GyroscopePermission { get; set; }
        IPermissionsPolicyPermissionConfiguration HidPermission { get; set; }
        IPermissionsPolicyPermissionConfiguration IdleDetectionPermission { get; set; }
        IPermissionsPolicyPermissionConfiguration MagnetometerPermission { get; set; }
        IPermissionsPolicyPermissionConfiguration MicrophonePermission { get; set; }
        IPermissionsPolicyPermissionConfiguration MidiPermission { get; set; }
        IPermissionsPolicyPermissionConfiguration NavigationOverridePermission { get; set; }
        IPermissionsPolicyPermissionConfiguration PaymentPermission { get; set; }
        IPermissionsPolicyPermissionConfiguration PictureInPicturePermission { get; set; }
        IPermissionsPolicyPermissionConfiguration PublickeyCredentialsGetPermission { get; set; }
        IPermissionsPolicyPermissionConfiguration ScreenWakeLockPermission { get; set; }
        IPermissionsPolicyPermissionConfiguration SerialPermission { get; set; }
        IPermissionsPolicyPermissionConfiguration SyncXhrPermission { get; set; }
        IPermissionsPolicyPermissionConfiguration UsbPermission { get; set; }
        IPermissionsPolicyPermissionConfiguration WebSharePermission { get; set; }
        IPermissionsPolicyPermissionConfiguration XrSpatialTrackingPermission { get; set; }
    }
}
