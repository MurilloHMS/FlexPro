using System.Net.Http.Headers;

namespace FlexPro.Client.Services
{
    public class AuthDelegatingHandler : DelegatingHandler
    {

        private readonly LocalStorageService _storageService;

        public AuthDelegatingHandler(LocalStorageService storageService)
        {
            _storageService = storageService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _storageService.GetItemAsync("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
