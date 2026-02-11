using Application.UseCases.RegisterUser;
using Microsoft.AspNetCore.Mvc;
using WorkMosmApi.Models;

namespace WorkMosMApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IRegisterUserUseCase _registerUser;

        public UsersController(IRegisterUserUseCase registerUser)
        {
            _registerUser = registerUser;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            await _registerUser.RegisterUserAsync(request.Email, request.PasswordHash);
            return Ok();
        }
    }
}
