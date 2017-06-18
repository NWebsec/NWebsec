// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.Common.HttpHeaders;

namespace NWebsec.Core.SharedProject.Tests.HttpHeaders
{
    public partial class HeaderGeneratorTests
    {
        private readonly HeaderGenerator _generator;

        public HeaderGeneratorTests()
        {
            _generator = new HeaderGenerator();
        }
    }
}