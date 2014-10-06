// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NUnit.Framework;

namespace NWebsec.Owin.Tests.Unit
{
    [TestFixture]
    public class XRobotsTagOptionsTests
    {
        private XRobotsTagOptions _options;

        [SetUp]
        public void Setup()
        {
            _options = new XRobotsTagOptions();
        }
       
        [Test]
        public void NoIndex_UpdatesConfig()
        {
            _options.NoIndex();

            Assert.IsTrue(_options.Config.NoIndex);
        }

        [Test]
        public void NoFollow_UpdatesConfig()
        {
            _options.NoFollow();

            Assert.IsTrue(_options.Config.NoFollow);
        }

        [Test]
        public void NoSnippet_UpdatesConfig()
        {
            _options.NoSnippet();

            Assert.IsTrue(_options.Config.NoSnippet);
        }

        [Test]
        public void NoArchive_UpdatesConfig()
        {
            _options.NoArchive();

            Assert.IsTrue(_options.Config.NoArchive);
        }

        [Test]
        public void NoOdp_UpdatesConfig()
        {
            _options.NoOdp();

            Assert.IsTrue(_options.Config.NoOdp);
        }

        [Test]
        public void NoTranslate_UpdatesConfig()
        {
            _options.NoTranslate();

            Assert.IsTrue(_options.Config.NoTranslate);
        }

        [Test]
        public void NoImageIndex_UpdatesConfig()
        {
            _options.NoImageIndex();

            Assert.IsTrue(_options.Config.NoImageIndex);
        }
    }
}