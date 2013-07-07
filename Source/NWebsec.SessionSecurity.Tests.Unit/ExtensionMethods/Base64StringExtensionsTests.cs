// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NUnit.Framework;
using NWebsec.SessionSecurity.ExtensionMethods;

namespace NWebsec.SessionSecurity.Tests.Unit.ExtensionMethods
{
    [TestFixture]
    public class Base64StringExtensionsTests
    {
        [Test]
        public void AddBase64Padding_AddsCorrectPadding()
        {
            var bytes1 = Convert.ToBase64String(new byte[10]).Trim('=');
            var bytes2 = Convert.ToBase64String(new byte[11]).Trim('=');
            var bytes3 = Convert.ToBase64String(new byte[12]).Trim('=');
            var bytes4 = Convert.ToBase64String(new byte[13]).Trim('=');

            Assert.DoesNotThrow(() => Convert.FromBase64String(bytes1.AddBase64Padding()));
            Assert.DoesNotThrow(() => Convert.FromBase64String(bytes2.AddBase64Padding()));
            Assert.DoesNotThrow(() => Convert.FromBase64String(bytes3.AddBase64Padding()));
            Assert.DoesNotThrow(() => Convert.FromBase64String(bytes4.AddBase64Padding()));
        }
    }
}
