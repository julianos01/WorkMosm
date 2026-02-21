
using FluentResults;
using System.Net.Http.Json;
using WorkMosm.FrontEnd.Helpers;
using WorkMosm.FrontEnd.Models;

namespace WorkMosm.FrontEnd.Services
{
    public class LoginService : ILoginService
    {
        private readonly HttpClient _http;
        public LoginService(HttpClient http)
        {
            _http = http;
        }
        public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request)
        {
            var response = await _http.PostAsJsonAsync("/auth/login", request);
            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                return Result.Ok(loginResponse!);
            }
            var apiError = await response.Content.ReadFromJsonAsync<ApiValidationError>();
            var messages = apiError.ExtractMessages();
            return Result.Fail(messages);
        }
    }
}
