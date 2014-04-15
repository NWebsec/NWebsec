// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Owin
{
    public class XRobotsTagOptions
    {
        internal XRobotsTagConfiguration Config { get; private set; }

        internal XRobotsTagOptions()
        {
            Config = new XRobotsTagConfiguration();
        }

        public XRobotsTagOptions NoIndex()
        {
            Config.NoIndex = Config.Enabled = true;
            return this;
        }

        public XRobotsTagOptions NoFollow()
        {
            Config.NoFollow = Config.Enabled = true;
            return this;
        }

        public XRobotsTagOptions NoSnippet()
        {
            Config.NoSnippet = Config.Enabled = true;
            return this;
        }

        public XRobotsTagOptions NoArchive()
        {
            Config.NoArchive = Config.Enabled = true;
            return this;
        }

        public XRobotsTagOptions NoOdp()
        {
            Config.NoOdp = Config.Enabled = true;
            return this;
        }

        public XRobotsTagOptions NoTranslate()
        {
            Config.NoTranslate = Config.Enabled = true;
            return this;
        }

        public XRobotsTagOptions NoImageIndex()
        {
            Config.NoImageIndex = Config.Enabled = true;
            return this;
        }
    }
}