using Nancy.ViewEngines;
using NetDevPL.Logging;
using NetDevPL.Navigation;
using Ninject;
using System.Collections.Generic;

namespace NetDevPL.Modules.Videos
{
    public class Module : Ninject.Module
    {
        private ILogger _logger;

        public override void Load()
        {
            _logger = Kernel.Get<ILogFactory>().CreateLogger(Key);
            _logger.Info("Loaded");

            var navigationModel = Kernel.Get<INavigationModel>();
            navigationModel.Add(NavigationLinks);

            ResourceViewLocationProvider.RootNamespaces
                .Add(DeclaringAssembly, "NetDevPL.Modules.Videos.Views");
        }

        public override string Key
        {
            get { return "NetDevPL.Modules.Videos"; }
        }

        private INavigationLink[] _navigationLinks;

        public override IEnumerable<INavigationLink> NavigationLinks
        {
            get
            {
                return _navigationLinks ?? (_navigationLinks = new INavigationLink[]
            {
                new NavigationLink(10, "~/videos", "Videos"),
            });
            }
        }
    }
}