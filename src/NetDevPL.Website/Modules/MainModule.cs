// -------------------------------------------------------------------------------------------------------------------
// <copyright file="MainModule.cs" project="NetDevPL.Website" date="2014-06-23 13:24">
// 
// </copyright>
// -------------------------------------------------------------------------------------------------------------------

namespace NetDevPL.Website.Modules
{
    using Nancy;

    public class MainModule : NancyModule
    {
        public MainModule()
        {
            Get["/"] = parameters => "Hello .NET Developers Poland!";
        }
    }
}
