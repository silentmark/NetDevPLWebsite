using Nancy.ViewEngines;
using NetDevPL.Logging;
using NetDevPL.NancyFx;
using Ninject;
using System.Reflection;

namespace NetDevPL.Modules.Videos
{
    //cr:mmisztal1980
    public class Module : ModuleBase
    {
        private ILogger _logger;

        public override void Load()
        {
            _logger = Kernel.Get<ILogFactory>().CreateLogger(Key);
            _logger.Info("Loaded");

            ResourceViewLocationProvider.RootNamespaces
                .Add(this.GetType().Assembly, "NetDevPL.Modules.Videos.Views");
        }

        public override string Key
        {
            get { return "NetDevPL.Modules.Videos"; }
        }

        public override Assembly DeclaringAssembly
        {
            get { return GetType().Assembly; }
        }
    }
}