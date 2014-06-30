using Nancy;

namespace NetDevPL.Modules.Videos.Modules
{
    //cr:mmisztal1980
    public class VideosModule : NancyModule
    {
        public VideosModule()
            : base("/videos")
        {
            Get["/"] = parameters => View["Index"];
        }
    }
}