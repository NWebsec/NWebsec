// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Fluent;

namespace NWebsec.Owin
{
    public interface IFluentXRobotsTagOptions : IFluentInterface
    {
        XRobotsTagOptions NoIndex();
        XRobotsTagOptions NoFollow();
        XRobotsTagOptions NoSnippet();
        XRobotsTagOptions NoArchive();
        XRobotsTagOptions NoOdp();
        XRobotsTagOptions NoTranslate();
        XRobotsTagOptions NoImageIndex();
    }
}