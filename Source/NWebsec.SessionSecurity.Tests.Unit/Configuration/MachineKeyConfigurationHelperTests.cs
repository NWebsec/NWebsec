// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Configuration;
using NUnit.Framework;
using NWebsec.SessionSecurity.Configuration;

namespace NWebsec.SessionSecurity.Tests.Unit.Configuration
{
    [TestFixture]
    public class MachineKeyConfigurationHelperTests
    {
        private const string MachineKeyValidation = "0505050505050505050505050505050505050505050505050505050505050505";

        private readonly byte[] expectedMachineKey = new byte[] {
            0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
            0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
            0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
            0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05
        };

        [Test]
        public void GetMachineKey_DefaultMachineKeyConfig_ThrowsException()
        {
            var helper = new MachineKeyConfigurationHelper(new MachineKeySection());

            Assert.Throws<ApplicationException>(() => helper.GetMachineKey());
        }

        [Test]
        public void GetMachineKey_MachineKeyConfigured_ReturnsMachineKey()
        {
            var config = new MachineKeySection { ValidationKey = MachineKeyValidation };
            var helper = new MachineKeyConfigurationHelper(config);

            var key = helper.GetMachineKey();

            Assert.AreEqual(expectedMachineKey, key);
        }
    }
}
