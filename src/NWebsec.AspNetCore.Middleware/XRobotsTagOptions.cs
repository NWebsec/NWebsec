// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.Common.HttpHeaders.Configuration;

namespace NWebsec.AspNetCore.Middleware
{
    public class XRobotsTagOptions : IFluentXRobotsTagOptions
    {
        internal XRobotsTagOptions()
        {
            Config = new XRobotsTagConfiguration();
        }

        internal XRobotsTagConfiguration Config { get; }

        public IFluentXRobotsTagOptions NoIndex()
        {
            Config.NoIndex = Config.Enabled = true;
            return this;
        }

        public IFluentXRobotsTagOptions NoFollow()
        {
            Config.NoFollow = Config.Enabled = true;
            return this;
        }

        public IFluentXRobotsTagOptions NoSnippet()
        {
            Config.NoSnippet = Config.Enabled = true;
            return this;
        }

        public IFluentXRobotsTagOptions NoArchive()
        {
            Config.NoArchive = Config.Enabled = true;
            return this;
        }

        public IFluentXRobotsTagOptions NoOdp()
        {
            Config.NoOdp = Config.Enabled = true;
            return this;
        }

        public IFluentXRobotsTagOptions NoTranslate()
        {
            Config.NoTranslate = Config.Enabled = true;
            return this;
        }

        public IFluentXRobotsTagOptions NoImageIndex()
        {
            Config.NoImageIndex = Config.Enabled = true;
            return this;
        }
    }
}