using Application.UseCases.DeactivateUser;
using Application.UseCases.GetUserProfile;
using Application.UseCases.RegisterUser;
using Application.UseCases.UpdateUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkMosmApi.Models;

namespace WorkMosMApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            await _registerUser.ExecuteAsync(request.Email, request.PasswordHash);
            return Ok();
        }

        [HttpGet("getuserprofile")]
        public async Task<IActionResult> GetUserProfile(string email)
        {
            var response = await _getUserProfile.GetUserProfileAsync(email);
            return Ok(response);
        }

        [HttpPut("updateuserprofile")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateUserRequest request)
        {
            await _updateUserProfile.ExecuteAsync(request);
            return Ok();
        }

        [HttpPatch("deactivateuser")]
        public async Task<IActionResult> DeactivateUser(string email)
        {
            await _deactivateUser.ExecuteAsync(email);
            return Ok();
        }
    }
}
