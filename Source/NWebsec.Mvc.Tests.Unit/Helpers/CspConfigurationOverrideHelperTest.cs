// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NUnit.Framework;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Mvc.Tests.Unit.Helpers
{
    [TestFixture]
    public class CspConfigurationOverrideHelperTest : CspConfigurationOverrideHelperTestBase
    {
        protected override bool ReportOnly
        {
            get { return false; }
        }
       
        [Test]
        public void CloneElement_UnsafeEval_ClonesElement()
        {
            var directive = new CspDirectiveUnsafeEvalConfiguration
            {
                UnsafeEvalSrc = true,
                UnsafeInlineSrc = false,
                CustomSources = new string[] {}
            };
            var clone = (ICspDirectiveUnsafeEvalConfiguration)CspConfigurationOverrideHelper.CloneElement(directive);

            Assert.IsTrue(clone.UnsafeEvalSrc);
        }

        [Test]
        public void CloneElement_UnsafeInline_ClonesElement()
        {
            var directive = new CspDirectiveUnsafeInlineConfiguration
            {
                UnsafeInlineSrc = true,
                CustomSources = new string[] {}
            };
            var clone = (ICspDirectiveUnsafeInlineConfiguration)CspConfigurationOverrideHelper.CloneElement(directive);

            Assert.IsTrue(clone.UnsafeInlineSrc);
        }
    }
}