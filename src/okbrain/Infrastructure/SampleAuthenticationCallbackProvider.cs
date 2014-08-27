

namespace okbrain.Infrastructure
{
    using Models;
    using Nancy;
    using Nancy.SimpleAuthentication;

    public class SampleAuthenticationCallbackProvider : IAuthenticationCallbackProvider
    {
        public dynamic Process(NancyModule nancyModule, AuthenticateCallbackData model)
        {
            return nancyModule.View["AuthenticationCallback", model];
        }

        public dynamic OnRedirectToAuthenticationProviderError(NancyModule nancyModule, string errorMessage)
        {
            var model = new {
                ErrorMessage = errorMessage
            };

            return nancyModule.View["index", model];
        }
    }
}