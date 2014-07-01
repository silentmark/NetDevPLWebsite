using Nancy.ViewEngines;
using NetDevPL.Logging;
using NetDevPL.Navigation;
using Ninject;
using System.Collections.Generic;
using System.Reflection;

namespace NetDevPL.Modules.Videos
{
    //cr:mmisztal1980
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

        public override Assembly DeclaringAssembly
        {
            get { return GetType().Assembly; }
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