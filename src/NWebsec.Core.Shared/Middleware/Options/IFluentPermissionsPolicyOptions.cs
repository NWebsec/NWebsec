// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.Core.Common.Fluent;
using NWebsec.Core.Common.HttpHeaders.Configuration;

namespace NWebsec.Core.Common.Middleware.Options
{
    public interface IFluentPermissionsPolicyOptions : IFluentInterface
    {
        /// <summary>
        /// Configures the accelerometer permission.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the permission.</param>
        /// <returns>The current <see cref="PermissionsPolicyOptions" /> instance.</returns>
        IFluentPermissionsPolicyOptions AccelerometerSources(Action<IPermissionsPolicyPermissionConfiguration> configurer);

        /// <summary>
        /// Configures the ambient-light-sensor permission.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the permission.</param>
        /// <returns>The current <see cref="PermissionsPolicyOptions" /> instance.</returns>
        IFluentPermissionsPolicyOptions AmbientLightSensorSources(Action<IPermissionsPolicyPermissionConfiguration> configurer);

        /// <summary>
        /// Configures the autoplay permission.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the permission.</param>
        /// <returns>The current <see cref="PermissionsPolicyOptions" /> instance.</returns>
        IFluentPermissionsPolicyOptions AutoplaySources(Action<IPermissionsPolicyPermissionConfiguration> configurer);

        /// <summary>
        /// Configures the battery permission.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the permission.</param>
        /// <returns>The current <see cref="PermissionsPolicyOptions" /> instance.</returns>
        IFluentPermissionsPolicyOptions BatterySources(Action<IPermissionsPolicyPermissionConfiguration> configurer);

        /// <summary>
        /// Configures the camera permission.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the permission.</param>
        /// <returns>The current <see cref="PermissionsPolicyOptions" /> instance.</returns>
        IFluentPermissionsPolicyOptions CameraSources(Action<IPermissionsPolicyPermissionConfiguration> configurer);

        /// <summary>
        /// Configures the cross-origin-isolated permission.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the permission.</param>
        /// <returns>The current <see cref="PermissionsPolicyOptions" /> instance.</returns>
        IFluentPermissionsPolicyOptions CrossOriginIsolatedSources(Action<IPermissionsPolicyPermissionConfiguration> configurer);

        /// <summary>
        /// Configures the display-capture permission.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the permission.</param>
        /// <returns>The current <see cref="PermissionsPolicyOptions" /> instance.</returns>
        IFluentPermissionsPolicyOptions DisplayCaptureSources(Action<IPermissionsPolicyPermissionConfiguration> configurer);

        /// <summary>
        /// Configures the document-domain permission.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the permission.</param>
        /// <returns>The current <see cref="PermissionsPolicyOptions" /> instance.</returns>
        IFluentPermissionsPolicyOptions DocumentDomainSources(Action<IPermissionsPolicyPermissionConfiguration> configurer);

        /// <summary>
        /// Configures the encrypted-media permission.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the permission.</param>
        /// <returns>The current <see cref="PermissionsPolicyOptions" /> instance.</returns>
        IFluentPermissionsPolicyOptions EncryptedMediaSources(Action<IPermissionsPolicyPermissionConfiguration> configurer);

        /// <summary>
        /// Configures the execution-while-not-rendered permission.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the permission.</param>
        /// <returns>The current <see cref="PermissionsPolicyOptions" /> instance.</returns>
        IFluentPermissionsPolicyOptions ExecutionWhileNotRenderedSources(Action<IPermissionsPolicyPermissionConfiguration> configurer);

        /// <summary>
        /// Configures the execution-while-out-of-viewport permission.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the permission.</param>
        /// <returns>The current <see cref="PermissionsPolicyOptions" /> instance.</returns>
        IFluentPermissionsPolicyOptions ExecutionWhileOutOfViewportSources(Action<IPermissionsPolicyPermissionConfiguration> configurer);

        /// <summary>
        /// Configures the fullscreen permission.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the permission.</param>
        /// <returns>The current <see cref="PermissionsPolicyOptions" /> instance.</returns>
        IFluentPermissionsPolicyOptions FullscreenSources(Action<IPermissionsPolicyPermissionConfiguration> configurer);

        /// <summary>
        /// Configures the geolocation permission.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the permission.</param>
        /// <returns>The current <see cref="PermissionsPolicyOptions" /> instance.</returns>
        IFluentPermissionsPolicyOptions GeolocationSources(Action<IPermissionsPolicyPermissionConfiguration> configurer);

        /// <summary>
        /// Configures the gyroscope permission.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the permission.</param>
        /// <returns>The current <see cref="PermissionsPolicyOptions" /> instance.</returns>
        IFluentPermissionsPolicyOptions GyroscopeSources(Action<IPermissionsPolicyPermissionConfiguration> configurer);

        /// <summary>
        /// Configures the hid permission.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the permission.</param>
        /// <returns>The current <see cref="PermissionsPolicyOptions" /> instance.</returns>
        IFluentPermissionsPolicyOptions HidSources(Action<IPermissionsPolicyPermissionConfiguration> configurer);

        /// <summary>
        /// Configures the idle-detection permission.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the permission.</param>
        /// <returns>The current <see cref="PermissionsPolicyOptions" /> instance.</returns>
        IFluentPermissionsPolicyOptions IdleDetectionSources(Action<IPermissionsPolicyPermissionConfiguration> configurer);

        /// <summary>
        /// Configures the magnetometer permission.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the permission.</param>
        /// <returns>The current <see cref="PermissionsPolicyOptions" /> instance.</returns>
        IFluentPermissionsPolicyOptions MagnetometerSources(Action<IPermissionsPolicyPermissionConfiguration> configurer);

        /// <summary>
        /// Configures the microphone permission.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the permission.</param>
        /// <returns>The current <see cref="PermissionsPolicyOptions" /> instance.</returns>
        IFluentPermissionsPolicyOptions MicrophoneSources(Action<IPermissionsPolicyPermissionConfiguration> configurer);

        /// <summary>
        /// Configures the midi permission.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the permission.</param>
        /// <returns>The current <see cref="PermissionsPolicyOptions" /> instance.</returns>
        IFluentPermissionsPolicyOptions MidiSources(Action<IPermissionsPolicyPermissionConfiguration> configurer);

        /// <summary>
        /// Configures the navigation-override permission.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the permission.</param>
        /// <returns>The current <see cref="PermissionsPolicyOptions" /> instance.</returns>
        IFluentPermissionsPolicyOptions NavigationOverrideSources(Action<IPermissionsPolicyPermissionConfiguration> configurer);

        /// <summary>
        /// Configures the payment permission.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the permission.</param>
        /// <returns>The current <see cref="PermissionsPolicyOptions" /> instance.</returns>
        IFluentPermissionsPolicyOptions PaymentSources(Action<IPermissionsPolicyPermissionConfiguration> configurer);

        /// <summary>
        /// Configures the picture-in-picture permission.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the permission.</param>
        /// <returns>The current <see cref="PermissionsPolicyOptions" /> instance.</returns>
        IFluentPermissionsPolicyOptions PictureInPictureSources(Action<IPermissionsPolicyPermissionConfiguration> configurer);

        /// <summary>
        /// Configures the publickey-credentials-get permission.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the permission.</param>
        /// <returns>The current <see cref="PermissionsPolicyOptions" /> instance.</returns>
        IFluentPermissionsPolicyOptions PublickeyCredentialsGetSources(Action<IPermissionsPolicyPermissionConfiguration> configurer);

        /// <summary>
        /// Configures the screen-wake-lock permission.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the permission.</param>
        /// <returns>The current <see cref="PermissionsPolicyOptions" /> instance.</returns>
        IFluentPermissionsPolicyOptions ScreenWakeLockSources(Action<IPermissionsPolicyPermissionConfiguration> configurer);

        /// <summary>
        /// Configures the serial permission.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the permission.</param>
        /// <returns>The current <see cref="PermissionsPolicyOptions" /> instance.</returns>
        IFluentPermissionsPolicyOptions SerialSources(Action<IPermissionsPolicyPermissionConfiguration> configurer);

        /// <summary>
        /// Configures the sync-xhr permission.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the permission.</param>
        /// <returns>The current <see cref="PermissionsPolicyOptions" /> instance.</returns>
        IFluentPermissionsPolicyOptions SyncXhrSources(Action<IPermissionsPolicyPermissionConfiguration> configurer);

        /// <summary>
        /// Configures the usb permission.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the permission.</param>
        /// <returns>The current <see cref="PermissionsPolicyOptions" /> instance.</returns>
        IFluentPermissionsPolicyOptions UsbSources(Action<IPermissionsPolicyPermissionConfiguration> configurer);

        /// <summary>
        /// Configures the web-share permission.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the permission.</param>
        /// <returns>The current <see cref="PermissionsPolicyOptions" /> instance.</returns>
        IFluentPermissionsPolicyOptions WebShareSources(Action<IPermissionsPolicyPermissionConfiguration> configurer);

        /// <summary>
        /// Configures the xr-spatial-tracking permission.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the permission.</param>
        /// <returns>The current <see cref="PermissionsPolicyOptions" /> instance.</returns>
        IFluentPermissionsPolicyOptions XrSpatialTrackingSources(Action<IPermissionsPolicyPermissionConfiguration> configurer);
    }
}
