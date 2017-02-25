// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Xunit;

namespace NWebsec.AspNetCore.Middleware.Tests
{
    public class XRobotsTagOptionsTests
    {
        private readonly XRobotsTagOptions _options;

        public XRobotsTagOptionsTests()
        {
            _options = new XRobotsTagOptions();
        }
       
        [Fact]
        public void NoIndex_UpdatesConfig()
        {
            _options.NoIndex();

            Assert.True(_options.Config.NoIndex);
        }

        [Fact]
        public void NoFollow_UpdatesConfig()
        {
            _options.NoFollow();

            Assert.True(_options.Config.NoFollow);
        }

        [Fact]
        public void NoSnippet_UpdatesConfig()
        {
            _options.NoSnippet();

            Assert.True(_options.Config.NoSnippet);
        }

        [Fact]
        public void NoArchive_UpdatesConfig()
        {
            _options.NoArchive();

            Assert.True(_options.Config.NoArchive);
        }

        [Fact]
        public void NoOdp_UpdatesConfig()
        {
            _options.NoOdp();

            Assert.True(_options.Config.NoOdp);
        }

        [Fact]
        public void NoTranslate_UpdatesConfig()
        {
            _options.NoTranslate();

            Assert.True(_options.Config.NoTranslate);
        }

        [Fact]
        public void NoImageIndex_UpdatesConfig()
        {
            _options.NoImageIndex();

            Assert.True(_options.Config.NoImageIndex);
        }
    }
}