using FluentResults;
using WorkMosm.FrontEnd.Models;

namespace WorkMosm.FrontEnd.Services.Interfaces
{
    public interface IUsersService
    {
        Task<Result> RegisterAsync(RegisterUserRequest request);
    }
}
