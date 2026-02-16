using Application.UseCases.LoginUser;
using FluentValidation;
using Microsoft.Extensions.Logging;
using WorkMosm.Application.Interfaces.Security;
using WorkMosm.Application.UseCases.LoginUser.Records;
using WorkMosm.Domain.CustomExceptions;
using WorkMosm.Domain.Ports;

namespace WorkMosm.Application.UseCases.LoginUser
{
    public class LoginUserUseCase : ILoginUserUseCase
    {
        private readonly IUserRepository _users;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IValidator<LoginUserRequest> _validator;
        private readonly ILogger<LoginUserUseCase> _logger;

        public LoginUserUseCase(
            IUserRepository users,
            IPasswordHasher passwordHasher,
            ITokenGenerator tokenGenerator,
            IValidator<LoginUserRequest> validator,
            ILogger<LoginUserUseCase> logger)
        {
            _users = users;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
            _validator = validator;
            _logger = logger;
        }

        public async Task<LoginUserResult> ExecuteAsync(LoginUserRequest request)
        {
            await _validator.ValidateAndThrowAsync(request);
            _logger.LogInformation("Attempting to log in user with email: {Email}", request.Email);

            var user = await _users.GetByEmailAsync(request.Email);

            if (user is null)
                throw new InvalidCredentialsException();

            if (!user.IsActive)
                throw new UnauthorizedAccessException("Inactive User");

            var isValid = _passwordHasher.Verify(request.Password, user.PasswordHash);

            if (!isValid)
                throw new InvalidCredentialsException();

            var tokenResponse = _tokenGenerator.GenerateToken(user);

            return tokenResponse;
        }
    }
}
