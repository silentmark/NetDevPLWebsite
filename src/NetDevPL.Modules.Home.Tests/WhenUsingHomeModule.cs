using Nancy;
using Nancy.Testing;
using NetDevPL.AspNet;
using Xunit;

namespace NetDevPL.Modules.Home.Tests
{
    public class WhenUsingHomeModule
    {
        [Fact]
        public void ShouldReturnHttpOkForDefaultRoute()
        {
            using (var bootstrapper = new Bootstrapper())
            {
                var browser = new Browser(bootstrapper);
                var result = browser.Get("/", with => with.HttpRequest());

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            }
        }
    }
}