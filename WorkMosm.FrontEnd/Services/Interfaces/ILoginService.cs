using FluentResults;
using WorkMosm.FrontEnd.Models;

namespace WorkMosm.FrontEnd.Services
{
    public interface ILoginService
    {
        Task<Result<LoginResponse>> LoginAsync(LoginRequest request);
    }
}