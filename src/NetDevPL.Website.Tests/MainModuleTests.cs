// -------------------------------------------------------------------------------------------------------------------
// <copyright file="MainModuleTests.cs" project="NetDevPL.Website.Tests" date="2014-06-23 13:29">
//
// </copyright>
// -------------------------------------------------------------------------------------------------------------------

namespace NetDevPL.Website.Tests
{
    using Nancy;
    using Nancy.Testing;
    using NetDevPL.Website.Modules;
    using NUnit.Framework;

    [TestFixture]
    public class MainModuleTests
    {
        [Test]
        public void MainModule_accessingMainPage_shouldReturnPage()
        {
            // Arrange
            var browser = new Browser(with => with.Module<MainModule>());

            // Act
            var result = browser.Get("/", ctx => ctx.HttpRequest());

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            StringAssert.Contains("Hello .NET Developers Poland!", result.Body.AsString());
        }
    }
}