using Nancy;

namespace NetDevPL.Modules.Videos.Modules
{
    public class VideosModule : NancyModule
    {
        public VideosModule()
            : base("/videos")
        {
            Get["/"] = parameters => View["Index"];
        }
    }
}