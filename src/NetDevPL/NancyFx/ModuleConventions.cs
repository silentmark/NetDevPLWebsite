using Nancy;
using Nancy.Conventions;
using NetDevPL.Logging;
using System;
using System.Collections.Generic;

namespace NetDevPL.NancyFx
{
    /// <summary>
    /// Holds information about nancy conventions delivered from submodules. That information is used in the last stages of bootstrapper
    /// sequence in order to configure the NancyConventions.
    ///
    /// Provides logic in order to configure the nancy conventions during the bootstrapper sequence.
    /// </summary>
    public class ModuleConventions : Disposable, IModuleConventions
    {
        private readonly ILogger _logger;

        public ModuleConventions(ILogFactory logFactory)
        {
            _logger = logFactory.CreateLogger("ModuleConventions");
            StaticContentsConvetions = new Dictionary<string, ICollection<Func<NancyContext, string, Response>>>();
        }

        public IDictionary<string, ICollection<Func<NancyContext, string, Response>>> StaticContentsConvetions
        {
            get;
            private set;
        }

        public void ConfigureConventions(NancyConventions nancyConventions)
        {
            foreach (var kvp in StaticContentsConvetions)
            {
                _logger.Debug("Configuring static content conventions for : {0}", kvp.Key);

                foreach (var convention in kvp.Value)
                {
                    nancyConventions.StaticContentsConventions.Add(convention);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _logger.Dispose();
            }
        }
    }
}