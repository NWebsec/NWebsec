// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Xunit;
using NWebsec.Core.Common.HttpHeaders.Configuration;

namespace NWebsec.AspNetCore.Mvc.Tests.TestHelpers
{
    public class CspSandboxDirectiveConfigurationEqualityComparerTests
    {
        private readonly CspSandboxDirectiveConfigurationEqualityComparer _equalityComparer;

        public CspSandboxDirectiveConfigurationEqualityComparerTests()
        {
            _equalityComparer = new CspSandboxDirectiveConfigurationEqualityComparer();
        }

        [Fact]
        public void Equals_IdenticalDefaultConfigs_ReturnsTrue()
        {
            var result = _equalityComparer.Equals(new CspSandboxDirectiveConfiguration(), new CspSandboxDirectiveConfiguration());

            Assert.True(result);
        }

        [Fact]
        public void Equals_EnabledDiffers_ReturnsFalse()
        {
            var firstConfig = new CspSandboxDirectiveConfiguration { Enabled = false };
            var secondConfig = new CspSandboxDirectiveConfiguration { Enabled = true };

            Assert.False(_equalityComparer.Equals(firstConfig, secondConfig));
            Assert.False(_equalityComparer.Equals(secondConfig, firstConfig));
        }

        [Fact]
        public void Equals_AllowFormsDiffers_ReturnsFalse()
        {
            var firstConfig = new CspSandboxDirectiveConfiguration { AllowForms = false };
            var secondConfig = new CspSandboxDirectiveConfiguration { AllowForms = true };

            Assert.False(_equalityComparer.Equals(firstConfig, secondConfig));
            Assert.False(_equalityComparer.Equals(secondConfig, firstConfig));
        }

        [Fact]
        public void Equals_AllowModalsDiffers_ReturnsFalse()
        {
            var firstConfig = new CspSandboxDirectiveConfiguration { AllowModals = false };
            var secondConfig = new CspSandboxDirectiveConfiguration { AllowModals = true };

            Assert.False(_equalityComparer.Equals(firstConfig, secondConfig));
            Assert.False(_equalityComparer.Equals(secondConfig, firstConfig));
        }

        [Fact]
        public void Equals_AllowOrientationLockDiffers_ReturnsFalse()
        {
            var firstConfig = new CspSandboxDirectiveConfiguration { AllowOrientationLock = false };
            var secondConfig = new CspSandboxDirectiveConfiguration { AllowOrientationLock = true };

            Assert.False(_equalityComparer.Equals(firstConfig, secondConfig));
            Assert.False(_equalityComparer.Equals(secondConfig, firstConfig));
        }

        [Fact]
        public void Equals_AllowPointerLockDiffers_ReturnsFalse()
        {
            var firstConfig = new CspSandboxDirectiveConfiguration { AllowPointerLock = false };
            var secondConfig = new CspSandboxDirectiveConfiguration { AllowPointerLock = true };

            Assert.False(_equalityComparer.Equals(firstConfig, secondConfig));
            Assert.False(_equalityComparer.Equals(secondConfig, firstConfig));
        }

        [Fact]
        public void Equals_AllowPopupsDiffers_ReturnsFalse()
        {
            var firstConfig = new CspSandboxDirectiveConfiguration { AllowPopups = false };
            var secondConfig = new CspSandboxDirectiveConfiguration { AllowPopups = true };

            Assert.False(_equalityComparer.Equals(firstConfig, secondConfig));
            Assert.False(_equalityComparer.Equals(secondConfig, firstConfig));
        }

        [Fact]
        public void Equals_AllowPopupsToEscapeSandboxDiffers_ReturnsFalse()
        {
            var firstConfig = new CspSandboxDirectiveConfiguration { AllowPopupsToEscapeSandbox = false };
            var secondConfig = new CspSandboxDirectiveConfiguration { AllowPopupsToEscapeSandbox = true };

            Assert.False(_equalityComparer.Equals(firstConfig, secondConfig));
            Assert.False(_equalityComparer.Equals(secondConfig, firstConfig));
        }

        [Fact]
        public void Equals_AllowPresentationDiffers_ReturnsFalse()
        {
            var firstConfig = new CspSandboxDirectiveConfiguration { AllowPresentation = false };
            var secondConfig = new CspSandboxDirectiveConfiguration { AllowPresentation = true };

            Assert.False(_equalityComparer.Equals(firstConfig, secondConfig));
            Assert.False(_equalityComparer.Equals(secondConfig, firstConfig));
        }

        [Fact]
        public void Equals_AllowSameOriginDiffers_ReturnsFalse()
        {
            var firstConfig = new CspSandboxDirectiveConfiguration { AllowSameOrigin = false };
            var secondConfig = new CspSandboxDirectiveConfiguration { AllowSameOrigin = true };

            Assert.False(_equalityComparer.Equals(firstConfig, secondConfig));
            Assert.False(_equalityComparer.Equals(secondConfig, firstConfig));
        }

        [Fact]
        public void Equals_AllowScriptsDiffers_ReturnsFalse()
        {
            var firstConfig = new CspSandboxDirectiveConfiguration { AllowScripts = false };
            var secondConfig = new CspSandboxDirectiveConfiguration { AllowScripts = true };

            Assert.False(_equalityComparer.Equals(firstConfig, secondConfig));
            Assert.False(_equalityComparer.Equals(secondConfig, firstConfig));
        }

        [Fact]
        public void Equals_AllowTopNavigationDiffers_ReturnsFalse()
        {
            var firstConfig = new CspSandboxDirectiveConfiguration { AllowTopNavigation = false };
            var secondConfig = new CspSandboxDirectiveConfiguration { AllowTopNavigation = true };

            Assert.False(_equalityComparer.Equals(firstConfig, secondConfig));
            Assert.False(_equalityComparer.Equals(secondConfig, firstConfig));
        }
    }
}