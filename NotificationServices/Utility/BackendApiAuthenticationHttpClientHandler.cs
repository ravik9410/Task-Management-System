using Microsoft.AspNetCore.Authentication;
using System.Net.Http;
using System.Net.Http.Headers;

namespace NotificationServices.Utility
{
    public class BackendApiAuthenticationHttpClientHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _accessor;
        public BackendApiAuthenticationHttpClientHandler(IHttpContextAccessor httpFactory)
        {
            _accessor = httpFactory;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_accessor.HttpContext != null)
            {
                var token = await _accessor.HttpContext?.GetTokenAsync("access_token");
                //SD.token = token;
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            }
            return await base.SendAsync(request, cancellationToken);
        }

    }
}
