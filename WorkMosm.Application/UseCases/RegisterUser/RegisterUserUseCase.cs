using FluentValidation;
using Microsoft.Extensions.Logging;
using WorkMosm.Application.Interfaces.Security;
using WorkMosm.Domain.CustomExceptions;
using WorkMosm.Domain.Entities;
using WorkMosm.Domain.Ports;

namespace WorkMosm.Application.UseCases.RegisterUser
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IValidator<RegisterUserRequest> _validator;
        private readonly ILogger<RegisterUserUseCase> _logger;

        public RegisterUserUseCase(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IValidator<RegisterUserRequest> validator,
            ILogger<RegisterUserUseCase> logger)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _validator = validator;
            _logger = logger;
        }

        public async Task ExecuteAsync(RegisterUserRequest registerUserRequest)
        {
            await _validator.ValidateAndThrowAsync(registerUserRequest);
            _logger.LogInformation("Registering user with email: {Email}", registerUserRequest.Email);

            var existingUser = await _userRepository.GetByEmailAsync(registerUserRequest.Email);
            if (existingUser != null)
            {
                throw new UserAlreadyExistsException();
            }
            var user = new User(registerUserRequest.Email, _passwordHasher.Hash(registerUserRequest.Password));
            await _userRepository.AddAsync(user);
        }
    }
}
