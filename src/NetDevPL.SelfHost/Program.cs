using Nancy.Hosting.Self;
using System;

namespace NetDevPL.SelfHost
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var host = new NancyHost(new Uri("http://localhost:80")))
            {
                host.Start();
                Console.ReadLine();
                host.Stop();
            }
        }
    }
}