using FluentValidation;
using Microsoft.Extensions.Logging;
using WorkMosm.Application.Interfaces.Security;
using WorkMosm.Domain.CustomExceptions;
using WorkMosm.Domain.Ports;

namespace WorkMosm.Application.UseCases.UpdateUser
{
    public class UpdateUserProfileUseCase : IUpdateUserProfile
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<UpdateUserProfileUseCase> _logger;
        private readonly IValidator<UpdateUserRequest> _validator;

        public UpdateUserProfileUseCase(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            ILogger<UpdateUserProfileUseCase> logger,
            IValidator<UpdateUserRequest> validator)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task ExecuteAsync(UpdateUserRequest request, string? currentUserId)
        {
            await ValidateRequest(request, currentUserId);
            _logger.LogInformation("Updating user profile for user ID: {UserId}", request.Id);

            var user = await _userRepository.GetByIdAsync(request.Id)
                       ?? throw new UserNotFoundException(request.Id);

            string passwordToUpdate = user.PasswordHash;

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                passwordToUpdate = _passwordHasher.Hash(request.Password);
            }

            user.UpdateProfile(request.Email, passwordToUpdate ?? user.PasswordHash);

            await _userRepository.UpdateAsync(user);
        }

        private async Task ValidateRequest(UpdateUserRequest request, string? currentUserId)
        {
            await _validator.ValidateAndThrowAsync(request);

            if (request.Id != currentUserId)
                throw new UnauthorizedAccessException("Users can only update their own profile.");
        }
    }
}