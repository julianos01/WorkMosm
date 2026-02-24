using FluentResults;
using System.Net.Http.Json;
using WorkMosm.FrontEnd.Helpers;
using WorkMosm.FrontEnd.Models;
using WorkMosm.FrontEnd.Services.Interfaces;

namespace WorkMosm.FrontEnd.Services
{
    public class UsersService : IUsersService
    {
        private readonly HttpClient _http;
        public UsersService(HttpClient http)
        {
            _http = http;
        }
        public async Task<Result> RegisterAsync(RegisterUserRequest request)
        {
            var response = await _http.PostAsJsonAsync("/users/register", request);
            if (response.IsSuccessStatusCode)
            {
                return Result.Ok();
            }

            var apiError = await response.Content.ReadFromJsonAsync<ApiValidationError>();

            var messages = apiError.ExtractMessages();

            return Result.Fail(messages);
        }
    }
}
