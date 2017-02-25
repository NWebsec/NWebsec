// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Xunit;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration;

namespace NWebsec.AspNetCore.Middleware.Tests
{
    public class FluentCspSandboxDirectiveTests
    {
        private readonly FluentCspSandboxDirective _options;

        public FluentCspSandboxDirectiveTests()
        {
            _options = new FluentCspSandboxDirective();
        }

        [Fact]
        public void AllowForms_SetsAllowForms()
        {
            _options.AllowForms();

            Assert.True(((CspSandboxDirectiveConfiguration)_options).AllowForms);
        }

        [Fact]
        public void AllowModals_SetsAllowModals()
        {
            _options.AllowModals();

            Assert.True(((CspSandboxDirectiveConfiguration)_options).AllowModals);
        }

        [Fact]
        public void AllowOrientationLock_SetsAllowOrientationLock()
        {
            _options.AllowOrientationLock();

            Assert.True(((CspSandboxDirectiveConfiguration)_options).AllowOrientationLock);
        }

        [Fact]
        public void AllowPointerLock_SetsAllowPointerLock()
        {
            _options.AllowPointerLock();

            Assert.True(((CspSandboxDirectiveConfiguration)_options).AllowPointerLock);
        }

        [Fact]
        public void AllowPopups_SetsAllowPopups()
        {
            _options.AllowPopups();

            Assert.True(((CspSandboxDirectiveConfiguration)_options).AllowPopups);
        }

        [Fact]
        public void AllowPopupsToEscapeSandbox_SetsAllowPopupsToEscapeSandbox()
        {
            _options.AllowPopupsToEscapeSandbox();

            Assert.True(((CspSandboxDirectiveConfiguration)_options).AllowPopupsToEscapeSandbox);
        }

        [Fact]
        public void AllowPresentation_SetsAllowPresentation()
        {
            _options.AllowPresentation();

            Assert.True(((CspSandboxDirectiveConfiguration)_options).AllowPresentation);
        }

        [Fact]
        public void AllowSameOrigin_SetsAllowSameOrigin()
        {
            _options.AllowSameOrigin();

            Assert.True(((CspSandboxDirectiveConfiguration)_options).AllowSameOrigin);
        }

        [Fact]
        public void AllowScripts_SetsAllowScripts()
        {
            _options.AllowScripts();

            Assert.True(((CspSandboxDirectiveConfiguration)_options).AllowScripts);
        }

        [Fact]
        public void AllowTopNavigation_SetsAllowTopNavigation()
        {
            _options.AllowTopNavigation();

            Assert.True(((CspSandboxDirectiveConfiguration)_options).AllowTopNavigation);
        }
    }
}