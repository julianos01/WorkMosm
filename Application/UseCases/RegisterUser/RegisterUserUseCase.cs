using Application.Interfaces.Security;
using Domain.Entities;
using Domain.Ports;

namespace Application.UseCases.RegisterUser
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserUseCase(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task ExecuteAsync(string email, string passwordHash)
        {

            var existingUser = await _userRepository.GetByEmailAsync(email);
            if (existingUser != null)
            {
                throw new Exception("User already exists.");
            }
            var user = new User(email, _passwordHasher.Hash(passwordHash));
            await _userRepository.AddAsync(user);
        }
    }
}
