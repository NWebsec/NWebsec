// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using NWebsec.Modules.Configuration;
using NWebsec.Modules.Configuration.Validation;
using Xunit;

namespace NWebsec.AspNet.Classic.Tests.Modules.Configuration.Validation
{
    public class ReferrerPolicyValidatorTests
    {
        private readonly ReferrerPolicyValidator _validator;

        public ReferrerPolicyValidatorTests()
        {
            _validator = new ReferrerPolicyValidator();
        }

        [Fact]
        public void CanValidate_ReferrerPolicyConfigurationElement_ReturnsTrue()
        {
            Assert.True(_validator.CanValidate(typeof(ReferrerPolicyConfigurationElement)));
        }

        [Theory]
        [MemberData(nameof(AllowedValuesXunit))]
        public void Validate_KnownValue_Returns(string allowedValue)
        {
            var config = new ReferrerPolicyConfigurationElement { ConfigPolicy = allowedValue };
            _validator.Validate(config);
        }

        [Fact]
        public void Validate_UnknownValue_Throws()
        {
            var config = new ReferrerPolicyConfigurationElement { ConfigPolicy = "yolo" };
            Assert.Throws<ConfigurationErrorsException>(() => _validator.Validate(config));
        }

        public static IEnumerable<object[]> AllowedValuesXunit => AllowedValues.Select(v => new object[] { v });

        public static IEnumerable<string> AllowedValues
        {
            get
            {
                yield return "no-referrer";
                yield return "no-referrer-when-downgrade";
                yield return "origin";
                yield return "origin-when-cross-origin";
                yield return "same-origin";
                yield return "strict-origin";
                yield return "strict-origin-when-cross-origin";
                yield return "unsafe-url";
            }
        }
    }
}
