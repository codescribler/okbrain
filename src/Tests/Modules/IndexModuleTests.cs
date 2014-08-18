using Nancy;
using Nancy.Testing;
using Xunit;
using okbrain.Modules;

namespace Tests.Modules
{
    public class IndexModuleTests
    {
        [Fact]
        public void LandingPage_should_exist()
        {
            // Given
            var bootstrapper = new DefaultNancyBootstrapper();
            var browser = new Browser(bootstrapper, defaults: to => to.Accept("text/html"));

            // When
            var result = browser.Get("/", with =>
            {
                with.HttpRequest();
            });

            // Then
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public void SubscribeOnHomePage_Bot_SuccessIsFalse()
        {
            // Given
            var bootstrapper = new DefaultNancyBootstrapper();
            var browser = new Browser(bootstrapper, defaults: to => to.Accept("application/json"));

            var result = browser.Post("/", with =>
                                               {
                                                   with.FormValue("email", "bob@bob.com");
                                                   with.FormValue("real", "dodgy");
                                                   with.HttpRequest();
                                               });

            // Then
            var commonResult = result.Body.DeserializeJson<CommonResult>();
            Assert.False(commonResult.Success);
        }

        [Fact]
        public void SubscribeOnHomePage_WithJustEmail_Success()
        {
            // Given
            var bootstrapper = new DefaultNancyBootstrapper();
            var browser = new Browser(bootstrapper, defaults: to => to.Accept("application/json"));

            var result = browser.Post("/", with =>
            {
                with.FormValue("email", "bob@bob.com");
                with.FormValue("real", "");
                with.HttpRequest();
            });

            // Then
            var commonResult = result.Body.DeserializeJson<CommonResult>();
            Assert.True(commonResult.Success);
        }

        [Fact]
        public void SubscribeOnHomePage_WithDuplicateEmail_AlreadySubscribedMessage()
        {
            // Given
            var bootstrapper = new DefaultNancyBootstrapper();
            var browser = new Browser(bootstrapper, defaults: to => to.Accept("application/json"));

            var result = browser.Post("/", with =>
            {
                with.FormValue("email", "bob@bob.com");
                with.FormValue("real", "");
                with.HttpRequest();
            });

            result = browser.Post("/", with =>
            {
                with.FormValue("email", "bob@bob.com");
                with.FormValue("real", "");
                with.HttpRequest();
            });

            // Then
            var commonResult = result.Body.DeserializeJson<CommonResult>();

            Assert.True(commonResult.Success);
            Assert.Equal("Already subscribed", commonResult.Message);
        }
    }
}
