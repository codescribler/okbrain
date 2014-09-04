

using okbrain.Services;

namespace okbrain.Infrastructure
{
    using Models;
    using Nancy;
    using Nancy.SimpleAuthentication;

    public class SampleAuthenticationCallbackProvider : IAuthenticationCallbackProvider
    {
        private IAuthenticationService _authenticationService;

        public SampleAuthenticationCallbackProvider(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public dynamic Process(NancyModule nancyModule, AuthenticateCallbackData model)
        {
            string providerName = model.AuthenticatedClient.ProviderName;
            //model.AuthenticatedClient.UserInformation.Id

            

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