// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NUnit.Framework;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration;

namespace NWebsec.AspNetCore.Middleware.Tests
{
    [TestFixture]
    public class FluentCspSandboxDirectiveTests
    {
        private FluentCspSandboxDirective _options;

        [SetUp]
        public void Setup()
        {
            _options = new FluentCspSandboxDirective();
        }

        [Test]
        public void AllowForms_SetsAllowForms()
        {
            _options.AllowForms();

            Assert.IsTrue(((CspSandboxDirectiveConfiguration)_options).AllowForms);
        }

        [Test]
        public void AllowModals_SetsAllowModals()
        {
            _options.AllowModals();

            Assert.IsTrue(((CspSandboxDirectiveConfiguration)_options).AllowModals);
        }

        [Test]
        public void AllowOrientationLock_SetsAllowOrientationLock()
        {
            _options.AllowOrientationLock();

            Assert.IsTrue(((CspSandboxDirectiveConfiguration)_options).AllowOrientationLock);
        }

        [Test]
        public void AllowPointerLock_SetsAllowPointerLock()
        {
            _options.AllowPointerLock();

            Assert.IsTrue(((CspSandboxDirectiveConfiguration)_options).AllowPointerLock);
        }

        [Test]
        public void AllowPopups_SetsAllowPopups()
        {
            _options.AllowPopups();

            Assert.IsTrue(((CspSandboxDirectiveConfiguration)_options).AllowPopups);
        }

        [Test]
        public void AllowPopupsToEscapeSandbox_SetsAllowPopupsToEscapeSandbox()
        {
            _options.AllowPopupsToEscapeSandbox();

            Assert.IsTrue(((CspSandboxDirectiveConfiguration)_options).AllowPopupsToEscapeSandbox);
        }

        [Test]
        public void AllowPresentation_SetsAllowPresentation()
        {
            _options.AllowPresentation();

            Assert.IsTrue(((CspSandboxDirectiveConfiguration)_options).AllowPresentation);
        }

        [Test]
        public void AllowSameOrigin_SetsAllowSameOrigin()
        {
            _options.AllowSameOrigin();

            Assert.IsTrue(((CspSandboxDirectiveConfiguration)_options).AllowSameOrigin);
        }

        [Test]
        public void AllowScripts_SetsAllowScripts()
        {
            _options.AllowScripts();

            Assert.IsTrue(((CspSandboxDirectiveConfiguration)_options).AllowScripts);
        }

        [Test]
        public void AllowTopNavigation_SetsAllowTopNavigation()
        {
            _options.AllowTopNavigation();

            Assert.IsTrue(((CspSandboxDirectiveConfiguration)_options).AllowTopNavigation);
        }
    }
}