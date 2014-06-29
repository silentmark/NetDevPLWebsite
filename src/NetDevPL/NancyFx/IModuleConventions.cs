using Nancy;
using Nancy.Conventions;
using System;
using System.Collections.Generic;

namespace NetDevPL.NancyFx
{
    //cr:mmisztal1980
    public interface IModuleConventions : IDisposable
    {
        IDictionary<string, ICollection<Func<NancyContext, string, Response>>> StaticContentsConvetions { get; }

        void AddStaticContentConvention(string moduleKey, Func<NancyContext, string, Response> convention);

        void ConfigureConventions(NancyConventions nancyConventions);
    }
}