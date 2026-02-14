using Application.UseCases.LoginUser;
using Microsoft.AspNetCore.Mvc;
using WorkMosm.Api.Models.Errors;
using WorkMosm.Application.UseCases.LoginUser.Records;

namespace WorkMosm.Api.Controllers
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
        [ProducesResponseType(typeof(LoginUserResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
        {
            var result = await _loginUser.ExecuteAsync(request);
            return Ok(result);
        }
    }
}
