using Application.Interfaces.Security;
using Application.UseCases.UpdateUser;
using Domain.CustomExceptions;
using Domain.Ports;

namespace Application.UseCases.UpdateUserProfile
{
    public class UpdateUserProfileUseCase : IUpdateUserProfile
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UpdateUserProfileUseCase(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        public async Task ExecuteAsync(UpdateUserRequest request, string? currentUserId)
        {
            ValidateRequest(request, currentUserId);

            var user = await _userRepository.GetByIdAsync(request.Id)
                       ?? throw new UserNotFoundException(request.Id);

            string passwordToUpdate = user.PasswordHash;

            if (!string.IsNullOrWhiteSpace(request.PasswordHash))
            {
                passwordToUpdate = _passwordHasher.Hash(request.PasswordHash);
            }

            user.UpdateProfile(request.Email, passwordToUpdate ?? user.PasswordHash);

            await _userRepository.UpdateAsync(user);
        }

        private static void ValidateRequest(UpdateUserRequest request, string? currentUserId)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.Id))
                throw new ArgumentException("Parameter 'Id' cannot be null or empty.");

            if (request.Id != currentUserId)
                throw new UnauthorizedAccessException("Users can only update their own profile.");
        }
    }
}