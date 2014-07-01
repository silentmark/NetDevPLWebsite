using NetDevPL.Navigation;
using Ninject.Modules;
using System.Collections.Generic;
using System.Reflection;

namespace NetDevPL.Ninject
{
    // cr:mmisztal1980
    public abstract class Module : NinjectModule
    {
        public abstract string Key { get; }

        public abstract Assembly DeclaringAssembly { get; }

        public abstract IEnumerable<INavigationLink> NavigationLinks { get; }
    }
}