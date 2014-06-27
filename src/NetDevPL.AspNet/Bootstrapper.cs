using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Ninject;
using Nancy.ViewEngines;
using Nancy.ViewEngines.Razor;
using NetDevPL.Logging;
using Ninject;
using Ninject.Extensions.NamedScope;
using System;
using System.Collections.Generic;

namespace NetDevPL.AspNet
{
    public class Bootstrapper : NinjectNancyBootstrapper
    {
        protected override void ApplicationStartup(IKernel container, IPipelines pipelines)
        {
            // No registrations should be performed in here, however you may
            // resolve things that are needed during application startup.
            StaticConfiguration.CaseSensitive = true;
            StaticConfiguration.DisableErrorTraces = false;
            StaticConfiguration.EnableRequestTracing = true;

            ResourceViewLocationProvider.Ignore
                .Add(typeof(Nancy.ViewEngines.Razor.RazorViewEngine).Assembly);
        }

        protected override void ConfigureApplicationContainer(IKernel kernel)
        {
            // Pre-configure the kernel
            kernel.Bind<ILogFactory>().To<LogFactory>().InSingletonScope();
            kernel.Bind<ILogger>().To<NLogLogger>().InParentScope();            // All ILoggers are disposed when parent objects are GC'd

            // Load modules
            // (!) Assembly name MUST NOT start with "Nancy"
            kernel.Load("NetDevPL.Modules.*.dll");
        }

        //protected override void ConfigureRequestContainer(IKernel container, NancyContext context)
        //{
        //    // Perform registrations that should have a request lifetime
        //}

        //protected override void RequestStartup(IKernel container, IPipelines pipelines, NancyContext context)
        //{
        //    // No registrations should be performed in here, however you may
        //    // resolve things that are needed during request startup.
        //}

        protected override NancyInternalConfiguration InternalConfiguration
        {
            get
            {
                return NancyInternalConfiguration.WithOverrides(nic => nic.ViewLocationProvider = typeof(ResourceViewLocationProvider));
            }
        }

        protected override IEnumerable<Type> ViewEngines
        {
            get { yield return typeof(RazorViewEngine); }
        }
    }
}