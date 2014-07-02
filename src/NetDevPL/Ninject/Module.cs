using NetDevPL.Navigation;
using Ninject.Modules;
using System.Collections.Generic;
using System.Reflection;

namespace NetDevPL.Ninject
{
    public abstract class Module : NinjectModule
    {
        public abstract string Key { get; }

        public Assembly DeclaringAssembly
        {
            get { return GetType().Assembly; }
        }

        public abstract IEnumerable<INavigationLink> NavigationLinks { get; }
    }
}