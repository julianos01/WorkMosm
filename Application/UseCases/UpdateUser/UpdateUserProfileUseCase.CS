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

        public async Task ExecuteAsync(UpdateUserRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.Id))
                throw new ArgumentException("Parameter 'Id' cannot be null or empty.", nameof(request.Id));

            var user = await _userRepository.GetByIdAsync(request.Id)
                       ?? throw new UserNotFoundException(request.Id);

            string? newPasswordHash = !string.IsNullOrWhiteSpace(request.PasswordHash)? _passwordHasher.Hash(request.PasswordHash): null;
            user.UpdateProfile(request.Email, newPasswordHash??user.PasswordHash);

            await _userRepository.UpdateAsync(user);
        }
    }
}