// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NUnit.Framework;
using NWebsec.AspNetCore.Core.HttpHeaders;

namespace NWebsec.Core.Tests.HttpHeaders
{
    [TestFixture]
    public partial class HeaderGeneratorTests
    {
        private HeaderGenerator _generator;

        [SetUp]
        public void Setup()
        {
            _generator = new HeaderGenerator();
        }
        
    }
}