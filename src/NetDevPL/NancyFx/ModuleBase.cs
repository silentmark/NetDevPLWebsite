using Ninject.Modules;
using System.Reflection;

namespace NetDevPL.NancyFx
{
    // cr:mmisztal1980
    public abstract class ModuleBase : NinjectModule
    {
        public abstract string Key { get; }

        public abstract Assembly DeclaringAssembly { get; }
    }
}