using Nancy.ViewEngines;
using NetDevPL.Logging;
using NetDevPL.NancyFx;
using NetDevPL.Navigation;
using Ninject;
using System.Collections.Generic;
using System.Linq;

namespace NetDevPL.Modules.Shell
{
    public class Module : Ninject.Module
    {
        private ILogger _logger;

        public override string Key
        {
            get { return "NetDevPL.Modules.Shell"; }
        }

        public override void Load()
        {
            _logger = Kernel.Get<ILogFactory>().CreateLogger(Key);
            _logger.Info("Loaded");

            var moduleConventions = Kernel.Get<IModuleConventions>();
            moduleConventions.AddStaticContentConvention(Key, EmbeddedStaticContentConventionBuilder.AddDirectory("/Content", DeclaringAssembly, "Content"));
            moduleConventions.AddStaticContentConvention(Key, EmbeddedStaticContentConventionBuilder.AddDirectory("/scripts", DeclaringAssembly, "scripts"));
            moduleConventions.AddStaticContentConvention(Key, EmbeddedStaticContentConventionBuilder.AddDirectory("/fonts", DeclaringAssembly, "fonts"));

            var navigationModel = Kernel.Get<INavigationModel>();
            navigationModel.Add(NavigationLinks);

            ResourceViewLocationProvider.RootNamespaces
                .Add(this.GetType().Assembly, "NetDevPL.Modules.Shell.Views");
        }

        // Shell doesn't contain any navigation links
        public override IEnumerable<INavigationLink> NavigationLinks
        {
            get { return Enumerable.Empty<INavigationLink>(); }
        }
    }
}