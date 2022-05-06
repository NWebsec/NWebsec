using System;
using System.Collections.Generic;
using System.Text;
using NWebsec.Core.Common.HttpHeaders;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using Xunit;

namespace NWebsec.Core.SharedProject.Tests.HttpHeaders
{
    public class HeaderGeneratorPermissionsPolicyTests
    {
        private readonly HeaderGenerator _generator;
        private const string PpHeaderName = "Permissions-Policy";

        public HeaderGeneratorPermissionsPolicyTests()
        {
            _generator = new HeaderGenerator();
        }

        [Fact]
        public void CreatePermissionsPolicyResult_Disabled_ReturnsEmptyResults()
        {
            var permissionsPolicyConfiguration = new PermissionsPolicyConfiguration{Enabled = false};
            var result = _generator.CreatePermissionsPolicyResult(permissionsPolicyConfiguration);

            Assert.Null(result);
        }

        [Fact]
        public void CreatePermissionsPolicyResult_EnabledButNoPermissionsEnabled_ReturnsEmptyResults()
        {
            var permissionsPolicyConfiguration = new PermissionsPolicyConfiguration { Enabled = true };
            var result = _generator.CreatePermissionsPolicyResult(permissionsPolicyConfiguration);

            Assert.Null(result);
        }

        [Fact]
        public void CreatePermissionsPolicyResult_EnabledWithAccelerometer_ReturnsPermissionsPolicyWithAccelerometer()
        {
            var permissionsPolicyConfiguration = new PermissionsPolicyConfiguration
            {
                Enabled = true,
                AccelerometerPermission = { SelfSrc = true}
            };
            var result = _generator.CreatePermissionsPolicyResult(permissionsPolicyConfiguration);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(PpHeaderName, result.Name);
            Assert.Equal("accelerometer=(self)", result.Value);
        }

        [Fact]
        public void CreatePermissionsPolicyResult_EnabledWithAmbientLightSensor_ReturnsPermissionsPolicyWithAmbientLightSensor()
        {
            var permissionsPolicyConfiguration = new PermissionsPolicyConfiguration
            {
                Enabled = true,
                AmbientLightSensorPermission = { NoneSrc = true }
            };
            var result = _generator.CreatePermissionsPolicyResult(permissionsPolicyConfiguration);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(PpHeaderName, result.Name);
            Assert.Equal("ambient-light-sensor=()", result.Value);
        }

        [Fact]
        public void CreatePermissionsPolicyResult_EnabledWithAutoplay_ReturnsPermissionsPolicyWithAutoplay()
        {
            var permissionsPolicyConfiguration = new PermissionsPolicyConfiguration
            {
                Enabled = true,
                AutoplayPermission = { AllSrc = true }
            };
            var result = _generator.CreatePermissionsPolicyResult(permissionsPolicyConfiguration);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(PpHeaderName, result.Name);
            Assert.Equal("autoplay=(*)", result.Value);
        }

        [Fact]
        public void CreatePermissionsPolicyResult_EnabledWithBattery_ReturnsPermissionsPolicyWithBattery()
        {
            var permissionsPolicyConfiguration = new PermissionsPolicyConfiguration
            {
                Enabled = true,
                BatteryPermission = { CustomSources = new List<string>{"https://example.com"} }
            };
            var result = _generator.CreatePermissionsPolicyResult(permissionsPolicyConfiguration);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(PpHeaderName, result.Name);
            Assert.Equal("battery=(\"https://example.com\")", result.Value);
        }

        [Fact]
        public void CreatePermissionsPolicyResult_EnabledWithCamera_ReturnsPermissionsPolicyWithCamera()
        {
            var permissionsPolicyConfiguration = new PermissionsPolicyConfiguration
            {
                Enabled = true,
                CameraPermission = { NoneSrc = true }
            };
            var result = _generator.CreatePermissionsPolicyResult(permissionsPolicyConfiguration);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(PpHeaderName, result.Name);
            Assert.Equal("camera=()", result.Value);
        }

        [Fact]
        public void CreatePermissionsPolicyResult_EnabledWithCrossOriginIsolated_ReturnsPermissionsPolicyWithCrossOriginIsolated()
        {
            var permissionsPolicyConfiguration = new PermissionsPolicyConfiguration
            {
                Enabled = true,
                CrossOriginIsolatedPermission = { NoneSrc = true }
            };
            var result = _generator.CreatePermissionsPolicyResult(permissionsPolicyConfiguration);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(PpHeaderName, result.Name);
            Assert.Equal("cross-origin-isolated=()", result.Value);
        }

        [Fact]
        public void CreatePermissionsPolicyResult_EnabledWithDisplayCapture_ReturnsPermissionsPolicyWithDisplayCapture()
        {
            var permissionsPolicyConfiguration = new PermissionsPolicyConfiguration
            {
                Enabled = true,
                DisplayCapturePermission = { SelfSrc = true }
            };
            var result = _generator.CreatePermissionsPolicyResult(permissionsPolicyConfiguration);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(PpHeaderName, result.Name);
            Assert.Equal("display-capture=(self)", result.Value);
        }

        [Fact]
        public void CreatePermissionsPolicyResult_EnabledWithDocumentDomain_ReturnsPermissionsPolicyWithDocumentDomain()
        {
            var permissionsPolicyConfiguration = new PermissionsPolicyConfiguration
            {
                Enabled = true,
                DocumentDomainPermission = { SelfSrc = true }
            };
            var result = _generator.CreatePermissionsPolicyResult(permissionsPolicyConfiguration);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(PpHeaderName, result.Name);
            Assert.Equal("document-domain=(self)", result.Value);
        }

        [Fact]
        public void CreatePermissionsPolicyResult_EnabledWithEncryptedMedia_ReturnsPermissionsPolicyWithEncryptedMedia()
        {
            var permissionsPolicyConfiguration = new PermissionsPolicyConfiguration
            {
                Enabled = true,
                EncryptedMediaPermission = { SelfSrc = true }
            };
            var result = _generator.CreatePermissionsPolicyResult(permissionsPolicyConfiguration);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(PpHeaderName, result.Name);
            Assert.Equal("encrypted-media=(self)", result.Value);
        }

        [Fact]
        public void CreatePermissionsPolicyResult_EnabledWithExecutionWhileNotRendered_ReturnsPermissionsPolicyWithExecutionWhileNotRendered()
        {
            var permissionsPolicyConfiguration = new PermissionsPolicyConfiguration
            {
                Enabled = true,
                ExecutionWhileNotRenderedPermission = { SelfSrc = true }
            };
            var result = _generator.CreatePermissionsPolicyResult(permissionsPolicyConfiguration);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(PpHeaderName, result.Name);
            Assert.Equal("execution-while-not-rendered=(self)", result.Value);
        }

        [Fact]
        public void CreatePermissionsPolicyResult_EnabledWithExecutionWhileOutOfViewport_ReturnsPermissionsPolicyWithExecutionWhileOutOfViewport()
        {
            var permissionsPolicyConfiguration = new PermissionsPolicyConfiguration
            {
                Enabled = true,
                ExecutionWhileOutOfViewportPermission = { SelfSrc = true }
            };
            var result = _generator.CreatePermissionsPolicyResult(permissionsPolicyConfiguration);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(PpHeaderName, result.Name);
            Assert.Equal("execution-while-out-of-viewport=(self)", result.Value);
        }

        [Fact]
        public void CreatePermissionsPolicyResult_EnabledWithFullscreen_ReturnsPermissionsPolicyWithFullscreen()
        {
            var permissionsPolicyConfiguration = new PermissionsPolicyConfiguration
            {
                Enabled = true,
                FullscreenPermission = { SelfSrc = true }
            };
            var result = _generator.CreatePermissionsPolicyResult(permissionsPolicyConfiguration);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(PpHeaderName, result.Name);
            Assert.Equal("fullscreen=(self)", result.Value);
        }

        [Fact]
        public void CreatePermissionsPolicyResult_EnabledWithGeolocation_ReturnsPermissionsPolicyWithGeolocation()
        {
            var permissionsPolicyConfiguration = new PermissionsPolicyConfiguration
            {
                Enabled = true,
                GeolocationPermission = { SelfSrc = true }
            };
            var result = _generator.CreatePermissionsPolicyResult(permissionsPolicyConfiguration);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(PpHeaderName, result.Name);
            Assert.Equal("geolocation=(self)", result.Value);
        }

        [Fact]
        public void CreatePermissionsPolicyResult_EnabledWithGyroscope_ReturnsPermissionsPolicyWithGyroscope()
        {
            var permissionsPolicyConfiguration = new PermissionsPolicyConfiguration
            {
                Enabled = true,
                GyroscopePermission = { SelfSrc = true }
            };
            var result = _generator.CreatePermissionsPolicyResult(permissionsPolicyConfiguration);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(PpHeaderName, result.Name);
            Assert.Equal("gyroscope=(self)", result.Value);
        }

        [Fact]
        public void CreatePermissionsPolicyResult_EnabledWithHid_ReturnsPermissionsPolicyWithHid()
        {
            var permissionsPolicyConfiguration = new PermissionsPolicyConfiguration
            {
                Enabled = true,
                HidPermission = { AllSrc = true }
            };
            var result = _generator.CreatePermissionsPolicyResult(permissionsPolicyConfiguration);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(PpHeaderName, result.Name);
            Assert.Equal("hid=(*)", result.Value);
        }

        [Fact]
        public void CreatePermissionsPolicyResult_EnabledWithIdleDetection_ReturnsPermissionsPolicyWithIdleDetection()
        {
            var permissionsPolicyConfiguration = new PermissionsPolicyConfiguration
            {
                Enabled = true,
                IdleDetectionPermission = { AllSrc = true }
            };
            var result = _generator.CreatePermissionsPolicyResult(permissionsPolicyConfiguration);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(PpHeaderName, result.Name);
            Assert.Equal("idle-detection=(*)", result.Value);
        }

        [Fact]
        public void CreatePermissionsPolicyResult_EnabledWithMagnetometer_ReturnsPermissionsPolicyWithMagnetometer()
        {
            var permissionsPolicyConfiguration = new PermissionsPolicyConfiguration
            {
                Enabled = true,
                MagnetometerPermission = { AllSrc = true }
            };
            var result = _generator.CreatePermissionsPolicyResult(permissionsPolicyConfiguration);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(PpHeaderName, result.Name);
            Assert.Equal("magnetometer=(*)", result.Value);
        }

        [Fact]
        public void CreatePermissionsPolicyResult_EnabledWithMicrophone_ReturnsPermissionsPolicyWithMicrophone()
        {
            var permissionsPolicyConfiguration = new PermissionsPolicyConfiguration
            {
                Enabled = true,
                MicrophonePermission = { AllSrc = true }
            };
            var result = _generator.CreatePermissionsPolicyResult(permissionsPolicyConfiguration);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(PpHeaderName, result.Name);
            Assert.Equal("microphone=(*)", result.Value);
        }

        [Fact]
        public void CreatePermissionsPolicyResult_EnabledWithMidi_ReturnsPermissionsPolicyWithMidi()
        {
            var permissionsPolicyConfiguration = new PermissionsPolicyConfiguration
            {
                Enabled = true,
                MidiPermission = { AllSrc = true }
            };
            var result = _generator.CreatePermissionsPolicyResult(permissionsPolicyConfiguration);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(PpHeaderName, result.Name);
            Assert.Equal("midi=(*)", result.Value);
        }

        [Fact]
        public void CreatePermissionsPolicyResult_EnabledWithNavigationOverride_ReturnsPermissionsPolicyWithNavigationOverride()
        {
            var permissionsPolicyConfiguration = new PermissionsPolicyConfiguration
            {
                Enabled = true,
                NavigationOverridePermission = { NoneSrc = true }
            };
            var result = _generator.CreatePermissionsPolicyResult(permissionsPolicyConfiguration);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(PpHeaderName, result.Name);
            Assert.Equal("navigation-override=()", result.Value);
        }

        [Fact]
        public void CreatePermissionsPolicyResult_EnabledWithPayment_ReturnsPermissionsPolicyWithPayment()
        {
            var permissionsPolicyConfiguration = new PermissionsPolicyConfiguration
            {
                Enabled = true,
                PaymentPermission = { NoneSrc = true }
            };
            var result = _generator.CreatePermissionsPolicyResult(permissionsPolicyConfiguration);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(PpHeaderName, result.Name);
            Assert.Equal("payment=()", result.Value);
        }

        [Fact]
        public void CreatePermissionsPolicyResult_EnabledWithPictureInPicture_ReturnsPermissionsPolicyWithPictureInPicture()
        {
            var permissionsPolicyConfiguration = new PermissionsPolicyConfiguration
            {
                Enabled = true,
                PictureInPicturePermission = { NoneSrc = true }
            };
            var result = _generator.CreatePermissionsPolicyResult(permissionsPolicyConfiguration);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(PpHeaderName, result.Name);
            Assert.Equal("picture-in-picture=()", result.Value);
        }

        [Fact]
        public void CreatePermissionsPolicyResult_EnabledWithPublickeyCredentialsGet_ReturnsPermissionsPolicyWithPublickeyCredentialsGet()
        {
            var permissionsPolicyConfiguration = new PermissionsPolicyConfiguration
            {
                Enabled = true,
                PublickeyCredentialsGetPermission = { NoneSrc = true }
            };
            var result = _generator.CreatePermissionsPolicyResult(permissionsPolicyConfiguration);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(PpHeaderName, result.Name);
            Assert.Equal("publickey-credentials-get=()", result.Value);
        }

        [Fact]
        public void CreatePermissionsPolicyResult_EnabledWithScreenWakeLock_ReturnsPermissionsPolicyWithScreenWakeLock()
        {
            var permissionsPolicyConfiguration = new PermissionsPolicyConfiguration
            {
                Enabled = true,
                ScreenWakeLockPermission = { NoneSrc = true }
            };
            var result = _generator.CreatePermissionsPolicyResult(permissionsPolicyConfiguration);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(PpHeaderName, result.Name);
            Assert.Equal("screen-wake-lock=()", result.Value);
        }

        [Fact]
        public void CreatePermissionsPolicyResult_EnabledWithSerial_ReturnsPermissionsPolicyWithSerial()
        {
            var permissionsPolicyConfiguration = new PermissionsPolicyConfiguration
            {
                Enabled = true,
                SerialPermission = { NoneSrc = true }
            };
            var result = _generator.CreatePermissionsPolicyResult(permissionsPolicyConfiguration);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(PpHeaderName, result.Name);
            Assert.Equal("serial=()", result.Value);
        }

        [Fact]
        public void CreatePermissionsPolicyResult_EnabledWithSyncXhr_ReturnsPermissionsPolicyWithSyncXhr()
        {
            var permissionsPolicyConfiguration = new PermissionsPolicyConfiguration
            {
                Enabled = true,
                SyncXhrPermission = { NoneSrc = true }
            };
            var result = _generator.CreatePermissionsPolicyResult(permissionsPolicyConfiguration);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(PpHeaderName, result.Name);
            Assert.Equal("sync-xhr=()", result.Value);
        }

        [Fact]
        public void CreatePermissionsPolicyResult_EnabledWithUsb_ReturnsPermissionsPolicyWithUsb()
        {
            var permissionsPolicyConfiguration = new PermissionsPolicyConfiguration
            {
                Enabled = true,
                UsbPermission = { SelfSrc = true }
            };
            var result = _generator.CreatePermissionsPolicyResult(permissionsPolicyConfiguration);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(PpHeaderName, result.Name);
            Assert.Equal("usb=(self)", result.Value);
        }

        [Fact]
        public void CreatePermissionsPolicyResult_EnabledWithWebShare_ReturnsPermissionsPolicyWithWebShare()
        {
            var permissionsPolicyConfiguration = new PermissionsPolicyConfiguration
            {
                Enabled = true,
                WebSharePermission = { SelfSrc = true }
            };
            var result = _generator.CreatePermissionsPolicyResult(permissionsPolicyConfiguration);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(PpHeaderName, result.Name);
            Assert.Equal("web-share=(self)", result.Value);
        }

        [Fact]
        public void CreatePermissionsPolicyResult_EnabledWithXrSpatialTracking_ReturnsPermissionsPolicyWithXrSpatialTracking()
        {
            var permissionsPolicyConfiguration = new PermissionsPolicyConfiguration
            {
                Enabled = true,
                XrSpatialTrackingPermission = { SelfSrc = true }
            };
            var result = _generator.CreatePermissionsPolicyResult(permissionsPolicyConfiguration);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(PpHeaderName, result.Name);
            Assert.Equal("xr-spatial-tracking=(self)", result.Value);
        }

        [Fact]
        public void CreatePermissionsPolicyResult_EnabledWithMultiplePermissionsAndSources()
        {
            var permissionsPolicyConfiguration = new PermissionsPolicyConfiguration
            {
                Enabled = true,
                FullscreenPermission =
                {
                    SelfSrc = true, 
                    CustomSources = new List<string>{"https://example.com", "https://another.example.com"}
                },
                GeolocationPermission = {AllSrc = true},
                CameraPermission = {NoneSrc = true}
            };
            var result = _generator.CreatePermissionsPolicyResult(permissionsPolicyConfiguration);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(PpHeaderName, result.Name);
            Assert.Equal("camera=(), fullscreen=(self \"https://example.com\" \"https://another.example.com\"), geolocation=(*)", result.Value);
        }

        [Fact]
        public void CreatePermissionsPolicyResult_EnabledWithMultiplePermissions()
        {
            var permissionsPolicyConfiguration = new PermissionsPolicyConfiguration
            {
                Enabled = true,
                GeolocationPermission =
                {
                    SelfSrc = true,
                    CustomSources = new List<string>{"https://example.com"}
                },
                MicrophonePermission = { NoneSrc = true }
            };
            var result = _generator.CreatePermissionsPolicyResult(permissionsPolicyConfiguration);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(PpHeaderName, result.Name);
            Assert.Equal("geolocation=(self \"https://example.com\"), microphone=()", result.Value);
        }
    }
}
