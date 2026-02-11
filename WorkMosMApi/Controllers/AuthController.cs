using Application.UseCases.LoginUser;
using Application.UseCases.LoginUser.Records;
using Microsoft.AspNetCore.Mvc;

namespace WorkMosmApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILoginUserUseCase _loginUser;

        public AuthController(ILoginUserUseCase loginUser)
        {
            _loginUser = loginUser;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
        {
            var result = await _loginUser.ExecuteAsync(request);
            return Ok(result);
        }   
    }
}
