// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Linq;
using NUnit.Framework;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration;

namespace NWebsec.AspNetCore.Middleware.Tests
{
    [TestFixture]
    public class FluentCspPluginTypesDirectiveTests
    {
        private FluentCspPluginTypesDirective _options;

        [SetUp]
        public void Setup()
        {
            _options = new FluentCspPluginTypesDirective();
        }


        [Test]
        public void MediaTypes_NoParams_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => _options.MediaTypes());
        }

        [Test]
        public void MediaTypes_WithParams_SetsPluginTypes()
        {
            _options.MediaTypes("application/pdf", "application/vnd.ms-excel");
            var expectedSequence = new[] { "application/pdf", "application/vnd.ms-excel" };

            Assert.IsTrue(expectedSequence.SequenceEqual(((ICspPluginTypesDirectiveConfiguration)_options).MediaTypes), "Sequences did not match.");
        }

        [Test]
        public void MediaTypes_InvalidMediaTypes_ThrowsException()
        {
            Assert.Throws<ArgumentException>(()=> _options.MediaTypes("application /pdf", "applicazion/vnd.ms-excel"));

        }
    }
}