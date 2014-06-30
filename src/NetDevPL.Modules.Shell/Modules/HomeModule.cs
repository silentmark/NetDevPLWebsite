using Nancy;

namespace NetDevPL.Modules.Shell.Modules
{
    //cr:mmisztal1980
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = parameters => View["Index"];
        }
    }
}