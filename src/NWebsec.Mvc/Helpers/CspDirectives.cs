// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

namespace NWebsec.Mvc.Helpers
{
    public enum CspDirectives
    {
        DefaultSrc = 0,
        ScriptSrc = 1,
        ObjectSrc = 2,
        StyleSrc = 3,
        ImgSrc = 4,
        MediaSrc = 5,
        FrameSrc = 6,
        FontSrc = 7,
        ConnectSrc = 8,
        ReportUri = 9,
        FrameAncestors = 10,
        BaseUri = 11,
        ChildSrc = 12,
        FormAction = 13
    }
}