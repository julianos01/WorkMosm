using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using WorkMosm.FrontEnd.Models;

namespace WorkMosm.FrontEnd.Auth
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _js;
        private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
        private readonly HttpClient _http;

        public CustomAuthStateProvider(IJSRuntime js, HttpClient http)
        {
            _js = js;
            _http = http;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var json = await _js.InvokeAsync<string>("localStorage.getItem", "authToken");
            if (string.IsNullOrWhiteSpace(json))
                return new AuthenticationState(_anonymous);

            var data = JsonSerializer.Deserialize<LoginResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (data == null || data.Expiration < DateTime.UtcNow)
            {
                await _js.InvokeVoidAsync("localStorage.removeItem", "authToken");
                return new AuthenticationState(_anonymous);
            }

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", data.Token);

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(data.Token), "jwt")));
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];

            var jsonBytes = ParseBase64WithoutPadding(payload);

            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            if (keyValuePairs != null)
            {
                claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString() ?? "")));
            }

            return claims;
        }

        public async Task MarkUserAsAuthenticated(LoginResponse response)
        {
            var json = JsonSerializer.Serialize(response);
            await _js.InvokeVoidAsync("localStorage.setItem", "authToken", json);

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", response.Token);

            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(response.Token), "jwt"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public async Task MarkUserAsLoggedOut()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", "authToken");
            _http.DefaultRequestHeaders.Authorization = null;

            var authState = Task.FromResult(new AuthenticationState(_anonymous));
            NotifyAuthenticationStateChanged(authState);
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
