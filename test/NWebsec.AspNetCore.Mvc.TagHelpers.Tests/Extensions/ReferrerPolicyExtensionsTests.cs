// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Linq;
using Xunit;
using NWebsec.AspNetCore.Mvc.TagHelpers.Extensions;
using ReferrerPolicyCore = NWebsec.Core.Common.HttpHeaders.ReferrerPolicy;

namespace NWebsec.AspNetCore.Mvc.TagHelpers.Tests.Extensions
{
    public class ReferrerPolicyExtensionsTests
    {

        [Fact]
        public void MapToCoreType_UnknownValue_Throws()
        {
            const ReferrerPolicy invalid = (ReferrerPolicy)Int32.MaxValue;

            Assert.False(Enum.IsDefined(typeof(ReferrerPolicyCore), (ReferrerPolicyCore)invalid));
            Assert.Throws<ArgumentOutOfRangeException>(() => invalid.MapToCoreType());
        }

        [Fact]
        public void MapToCoreType_KnownValue_ValuesMatch()
        {
            //Disabled is not included in the tag helper version of the enum.
            var coreValues = Enum.GetValues(typeof(ReferrerPolicyCore)).Cast<ReferrerPolicyCore>().Where(p => p != ReferrerPolicyCore.Disabled).Select(p => (int)p);
            var mvcValues = Enum.GetValues(typeof(ReferrerPolicy)).Cast<ReferrerPolicy>().Select(p => (int)p);

            Assert.Equal(coreValues, mvcValues);
        }

        [Fact]
        public void MapToCoreType_KnownValue_NamesMatch()
        {
            //Disabled is not included in the tag helper version of the enum.
            var coreValues = Enum.GetNames(typeof(ReferrerPolicyCore)).Where(name => name != Enum.GetName(typeof(ReferrerPolicyCore), ReferrerPolicyCore.Disabled));
            var mvcValues = Enum.GetNames(typeof(ReferrerPolicy));

            Assert.Equal(coreValues, mvcValues);
        }
    }
}
