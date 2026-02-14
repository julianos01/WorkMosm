using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkMosm.Api.Models.Errors;
using WorkMosm.Application.UseCases.DeactivateUser;
using WorkMosm.Application.UseCases.GetUserProfile;
using WorkMosm.Application.UseCases.LoginUser.Records;
using WorkMosm.Application.UseCases.RegisterUser;
using WorkMosm.Application.UseCases.UpdateUser;
using WorkMosm.Infrastructure.Extensions;

namespace WorkMosm.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType(typeof(LoginUserResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public class UsersController : ControllerBase
    {
        private readonly IRegisterUserUseCase _registerUser;
        private readonly IGetUserProfileUseCase _getUserProfile;
        private readonly IUpdateUserProfile _updateUserProfile;
        private readonly IDeactivateUserUseCase _deactivateUser;

        public UsersController(IRegisterUserUseCase registerUser, IGetUserProfileUseCase getUserProfile, IUpdateUserProfile updateUserProfile,
                               IDeactivateUserUseCase deactivateUserUseCase)
        {
            _registerUser = registerUser;
            _getUserProfile = getUserProfile;
            _updateUserProfile = updateUserProfile;
            _deactivateUser = deactivateUserUseCase;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            await _registerUser.ExecuteAsync(request);
            return Ok();
        }

        [HttpGet("getuserprofile")]
        public async Task<IActionResult> GetUserProfile(string email)
        {
            var response = await _getUserProfile.ExecuteAsync(new GetUSerProfileRequest(email));
            return Ok(response);
        }

        [HttpPut("updateuserprofile")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateUserRequest request)
        {
            var currentUserId = User.GetUserId();

            await _updateUserProfile.ExecuteAsync(request, currentUserId);
            return Ok();
        }

        [HttpPatch("deactivateuser")]
        public async Task<IActionResult> DeactivateUser(string email)
        {
            DeactivateUserRequest request = new(email);
            await _deactivateUser.ExecuteAsync(request);
            return Ok();
        }
    }
}
