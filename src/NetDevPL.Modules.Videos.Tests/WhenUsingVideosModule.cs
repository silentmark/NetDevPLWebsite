using Nancy;
using Nancy.Testing;
using NetDevPL.AspNet;
using NUnit.Framework;

namespace NetDevPL.Modules.Videos.Tests
{
    [TestFixture]
    public class WhenUsingHomeModule
    {
        [Test]
        public void ShouldReturnHttpOkForDefaultRoute()
        {
            using (var bootstrapper = new Bootstrapper())
            {
                var browser = new Browser(bootstrapper);
                var result = browser.Get("/videos", with => with.HttpRequest());

                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            }
        }
    }
}