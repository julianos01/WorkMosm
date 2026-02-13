using Application.Interfaces.Security;
using Domain.Entities;
using Domain.Ports;
using FluentValidation;

namespace Application.UseCases.RegisterUser
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IValidator<RegisterUserRequest> _validator;

        public RegisterUserUseCase(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IValidator<RegisterUserRequest> validator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _validator = validator;
        }

        public async Task ExecuteAsync(RegisterUserRequest registerUserRequest)
        {
            await _validator.ValidateAndThrowAsync(registerUserRequest);

            var existingUser = await _userRepository.GetByEmailAsync(registerUserRequest.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("User already exists.");
            }
            var user = new User(registerUserRequest.Email, _passwordHasher.Hash(registerUserRequest.Password));
            await _userRepository.AddAsync(user);
        }
    }
}
