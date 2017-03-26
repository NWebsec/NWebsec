// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Linq;
using NWebsec.AspNetCore.Mvc.Extensions;
using Xunit;
using ReferrerPolicyCore = NWebsec.AspNetCore.Core.HttpHeaders.ReferrerPolicy;

namespace NWebsec.AspNetCore.Mvc.Tests.Extensions
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
            var coreValues = Enum.GetValues(typeof(ReferrerPolicyCore)).Cast<ReferrerPolicyCore>().Select(p => (int)p);
            var mvcValues = Enum.GetValues(typeof(ReferrerPolicy)).Cast<ReferrerPolicy>().Select(p => (int)p);

            Assert.Equal(coreValues, mvcValues);
        }

        [Fact]
        public void MapToCoreType_KnownValue_NamesMatch()
        {
            var coreValues = Enum.GetNames(typeof(ReferrerPolicyCore));
            var mvcValues = Enum.GetNames(typeof(ReferrerPolicy));

            Assert.Equal(coreValues, mvcValues);
        }
    }
}
