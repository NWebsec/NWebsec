// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using NWebsec.Modules.Configuration;
using NWebsec.Modules.Configuration.Validation;

namespace NWebsec.Tests.Unit.Modules.Configuration.Validation
{
    [TestFixture]
    public class RedirectSameHostConfigurationElementValidatorTests
    {
        private RedirectSameHostConfigurationElementValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new RedirectSameHostConfigurationElementValidator();
        }

       [Test]
        public void Validate_DisabledAndNoPorts_NoException()
        {
            var config = new RedirectSameHostConfigurationElement { Enabled = false };

            Assert.DoesNotThrow(() => _validator.Validate(config));
        }

        [Test]
        public void Validate_EnabledAndNoPorts_NoException()
        {
            var config = new RedirectSameHostConfigurationElement { Enabled = true };
            
            Assert.DoesNotThrow(() => _validator.Validate(config));
        }

        [Test]
        public void Validate_EnabledAndValidPort_NoException()
        {
            var config = new RedirectSameHostConfigurationElement { Enabled = true, HttpsPorts = "8000"};

            Assert.DoesNotThrow(() => _validator.Validate(config));
        }

        [Test]
        public void Validate_EnabledAndMultipleValidPorts()
        {
            var allPorts = String.Join(",", Enumerable.Range(1, 65535).Select(i => i.ToString(CultureInfo.InvariantCulture)).ToArray());
            var allValidPortsConfig = new RedirectSameHostConfigurationElement { Enabled = true, HttpsPorts = allPorts };
            var commaSeparatedConfig = new RedirectSameHostConfigurationElement { Enabled = true, HttpsPorts = "8000, 9000, 10000" };
            var condensedConfig = new RedirectSameHostConfigurationElement { Enabled = true, HttpsPorts = "8000,9000,10000" };
            var sloppyConfig = new RedirectSameHostConfigurationElement { Enabled = true, HttpsPorts = " 8000,   9000 , 10000 " };

            Assert.DoesNotThrow(() => _validator.Validate(allValidPortsConfig));
            Assert.DoesNotThrow(() => _validator.Validate(commaSeparatedConfig));
            Assert.DoesNotThrow(() => _validator.Validate(condensedConfig));
            Assert.DoesNotThrow(() => _validator.Validate(sloppyConfig));
        }

        [Test]
        public void Validate_DisabledAndMalformedPorts_NoException()
        {
            var couldntEnterValidPortAndGaveUpConfig = new RedirectSameHostConfigurationElement { Enabled = false, HttpsPorts = "notaport" };

            Assert.DoesNotThrow(() => _validator.Validate(couldntEnterValidPortAndGaveUpConfig));
        }

        [Test]
        public void Validate_EnabledAndMalformedPorts()
        {
            var couldntEnterValidPortButStillTryingConfig = new RedirectSameHostConfigurationElement { Enabled = true, HttpsPorts = "notaport" };
            var invalidCommaSeparatedStartConfig = new RedirectSameHostConfigurationElement { Enabled = true, HttpsPorts = ",8000, 9000, 10000" };
            var invalidCommaSeparatedMiddleConfig = new RedirectSameHostConfigurationElement { Enabled = true, HttpsPorts = "8000, 9000,, 10000" };
            var invalidCommaSeparatedEndConfig = new RedirectSameHostConfigurationElement { Enabled = true, HttpsPorts = "8000, 9000, 10000," };

            Assert.Throws<ConfigurationErrorsException>(() => _validator.Validate(couldntEnterValidPortButStillTryingConfig));
            Assert.Throws<ConfigurationErrorsException>(() => _validator.Validate(invalidCommaSeparatedStartConfig));
            Assert.Throws<ConfigurationErrorsException>(() => _validator.Validate(invalidCommaSeparatedMiddleConfig));
            Assert.Throws<ConfigurationErrorsException>(() => _validator.Validate(invalidCommaSeparatedEndConfig));
        }

        [Test]
        public void Validate_EnabledAndInvalidPorts()
        {
            var invalidLowPortConfig = new RedirectSameHostConfigurationElement { Enabled = true, HttpsPorts = "0,8000, 9000, 10000" };
            var invalidHighPortConfig = new RedirectSameHostConfigurationElement { Enabled = true, HttpsPorts = "8000, 9000,, 10000, 65536" };

            Assert.Throws<ConfigurationErrorsException>(() => _validator.Validate(invalidLowPortConfig));
            Assert.Throws<ConfigurationErrorsException>(() => _validator.Validate(invalidHighPortConfig));
        }

    }
}