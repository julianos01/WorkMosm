using Domain.CustomExceptions;
using Domain.Ports;

namespace Application.UseCases.DeactivateUser
{
    public class DeactivateUserUseCase : IDeactivateUserUseCase
    {
        private readonly IUserRepository _userRepository;

        public DeactivateUserUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task ExecuteAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Parameter 'email' cannot be null or empty.", nameof(email));

            var user = await _userRepository.GetByEmailAsync(email)
                       ?? throw new UserNotFoundException(email);

            user.Deactivate();

            await _userRepository.UpdateAsync(user);
        }
    }
}