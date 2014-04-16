// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Fluent;

namespace NWebsec.Owin
{
    public interface IFluentCspOptions : IFluentInterface
    {
        CspOptions DefaultSources(Action<ICspDirectiveConfiguration> configurer);
        CspOptions ScriptSources(Action<ICspDirectiveUnsafeEvalConfiguration> action);
        CspOptions ObjectSources(Action<ICspDirectiveConfiguration> action);
        CspOptions StyleSources(Action<ICspDirectiveUnsafeInlineConfiguration> action);
        CspOptions ImageSources(Action<ICspDirectiveConfiguration> action);
        CspOptions MediaSources(Action<ICspDirectiveConfiguration> action);
        CspOptions FrameSources(Action<ICspDirectiveConfiguration> action);
        CspOptions FontSources(Action<ICspDirectiveConfiguration> action);
        CspOptions ConnectSources(Action<ICspDirectiveConfiguration> action);
    }
}