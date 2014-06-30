using Nancy.ViewEngines;
using NetDevPL.Logging;
using Ninject;
using System.Reflection;

namespace NetDevPL.Modules.Home
{
    //cr:mmisztal1980
    public class Module : NetDevPL.NancyFx.Module
    {
        private ILogger _logger;

        public override string Key
        {
            get { return "NetDevPL.Modules.Home"; }
        }

        public override Assembly DeclaringAssembly
        {
            get { return GetType().Assembly; }
        }

        public override void Load()
        {
            _logger = Kernel.Get<ILogFactory>().CreateLogger(Key);
            _logger.Info("Loaded");

            ResourceViewLocationProvider.RootNamespaces
                .Add(this.GetType().Assembly, "NetDevPL.Modules.Home.Views");
        }
    }
}