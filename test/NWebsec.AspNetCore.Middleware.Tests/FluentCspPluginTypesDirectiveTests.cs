// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Linq;
using Xunit;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration;

namespace NWebsec.AspNetCore.Middleware.Tests
{
    public class FluentCspPluginTypesDirectiveTests
    {
        private readonly FluentCspPluginTypesDirective _options;

        public FluentCspPluginTypesDirectiveTests()
        {
            _options = new FluentCspPluginTypesDirective();
        }


        [Fact]
        public void MediaTypes_NoParams_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => _options.MediaTypes());
        }

        [Fact]
        public void MediaTypes_WithParams_SetsPluginTypes()
        {
            _options.MediaTypes("application/pdf", "application/vnd.ms-excel");
            var expectedSequence = new[] { "application/pdf", "application/vnd.ms-excel" };

            Assert.True(expectedSequence.SequenceEqual(((ICspPluginTypesDirectiveConfiguration)_options).MediaTypes), "Sequences did not match.");
        }

        [Fact]
        public void MediaTypes_InvalidMediaTypes_ThrowsException()
        {
            Assert.Throws<ArgumentException>(()=> _options.MediaTypes("application /pdf", "applicazion/vnd.ms-excel"));

        }
    }
}