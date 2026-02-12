using Application.Interfaces.Security;
using Application.UseCases.LoginUser.Records;
using Domain.CustomExceptions;
using Domain.Ports;

namespace Application.UseCases.LoginUser
{
    public class LoginUserUseCase : ILoginUserUseCase
    {
        private readonly IUserRepository _users;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;

        public LoginUserUseCase(IUserRepository users, IPasswordHasher passwordHasher, ITokenGenerator tokenGenerator)
        {
            _users = users;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<LoginUserResult> ExecuteAsync(LoginUserRequest request)
        {
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
