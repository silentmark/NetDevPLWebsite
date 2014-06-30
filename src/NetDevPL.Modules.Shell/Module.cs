using Nancy.ViewEngines;
using NetDevPL.Logging;
using NetDevPL.NancyFx;
using Ninject;
using System.Reflection;

namespace NetDevPL.Modules.Shell
{
    //cr:mmisztal1980
    public class Module : ModuleBase
    {
        private ILogger _logger;

        public override string Key
        {
            get { return "NetDevPL.Modules.Shell"; }
        }

        public override Assembly DeclaringAssembly
        {
            get { return GetType().Assembly; }
        }

        public override void Load()
        {
            _logger = Kernel.Get<ILogFactory>().CreateLogger(Key);
            _logger.Info("Loaded");

            var moduleConventions = Kernel.Get<IModuleConventions>();
            moduleConventions.AddStaticContentConvention(Key, EmbeddedStaticContentConventionBuilder.AddDirectory("/Content", DeclaringAssembly, "Content"));
            moduleConventions.AddStaticContentConvention(Key, EmbeddedStaticContentConventionBuilder.AddDirectory("/scripts", DeclaringAssembly, "scripts"));
            moduleConventions.AddStaticContentConvention(Key, EmbeddedStaticContentConventionBuilder.AddDirectory("/fonts", DeclaringAssembly, "fonts"));

            ResourceViewLocationProvider.RootNamespaces
                .Add(this.GetType().Assembly, "NetDevPL.Modules.Shell.Views");
        }
    }
}