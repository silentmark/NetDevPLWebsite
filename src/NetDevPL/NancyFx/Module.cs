using Ninject.Modules;

namespace NetDevPL.NancyFx
{
    public abstract class Module : NinjectModule
    {
        public abstract string Key { get; }
    }
}