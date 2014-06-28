using Nancy;
using Nancy.Conventions;
using System;
using System.Collections.Generic;

namespace NetDevPL.NancyFx
{
    public interface IModuleConventions : IDisposable
    {
        IDictionary<string, ICollection<Func<NancyContext, string, Response>>> StaticContentsConvetions { get; }

        void ConfigureConventions(NancyConventions nancyConventions);
    }
}