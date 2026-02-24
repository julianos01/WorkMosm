using Microsoft.JSInterop;
using WorkMosm.FrontEnd.Models;

namespace WorkMosm.FrontEnd.Auth
{
    public class AuthenticationHandler : DelegatingHandler
    {
        private readonly IJSRuntime _jsRuntime;

        public AuthenticationHandler(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var tokenJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
            if (!string.IsNullOrEmpty(tokenJson))
            {
                var token = System.Text.Json.JsonSerializer.Deserialize<LoginResponse>(tokenJson);
                if (token != null)
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Token);
                }
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
