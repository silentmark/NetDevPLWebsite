using Ninject.Modules;
using System.Reflection;

namespace NetDevPL.NancyFx
{
    // cr:mmisztal1980
    public abstract class Module : NinjectModule
    {
        public abstract string Key { get; }

        public abstract Assembly DeclaringAssembly { get; }

        public string[] ManifestResourceNames
        {
            get
            {
                return DeclaringAssembly.GetManifestResourceNames();
            }
        }
    }
}