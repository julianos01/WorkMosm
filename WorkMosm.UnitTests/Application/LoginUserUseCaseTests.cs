using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using WorkMosm.Application.Interfaces.Security;
using WorkMosm.Application.UseCases.LoginUser;
using WorkMosm.Application.UseCases.LoginUser.Records;
using WorkMosm.Application.Validators;
using WorkMosm.Domain.CustomExceptions;
using WorkMosm.Domain.Entities;
using WorkMosm.Domain.Ports;

namespace WorkMosm.UnitTests.Application
{
    public class LoginUserUseCaseTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<ITokenGenerator> _tokenGeneratorMock;
        private readonly LoginUserUseCase _useCase;
        private readonly LoginUserValidator _validator = new();
        private readonly ILogger<LoginUserUseCase> _logger;

        public LoginUserUseCaseTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _tokenGeneratorMock = new Mock<ITokenGenerator>();
            _logger = new Logger<LoginUserUseCase>(new LoggerFactory());


            _useCase = new LoginUserUseCase(
                _userRepositoryMock.Object,
                _passwordHasherMock.Object,
                _tokenGeneratorMock.Object,
                _validator,
                _logger);
        }

        [Fact]
        public async Task ExecuteAsync_WhenCredentialsAreValid_ReturnsToken()
        {
            // Arrange
            var request = new LoginUserRequest("test@example.com", "CorrectPassword123");
            var user = new User(request.Email, "hashed_password") { IsActive = true };
            var expectedResult = new LoginUserResult("fake-jwt-token", DateTime.UtcNow.AddHours(1), user.Id.ToString());

            _userRepositoryMock.Setup(r => r.GetByEmailAsync(request.Email)).ReturnsAsync(user);
            _passwordHasherMock.Setup(h => h.Verify(request.Password, user.PasswordHash)).Returns(true);
            _tokenGeneratorMock.Setup(g => g.GenerateToken(user)).Returns(expectedResult);

            // Act
            var result = await _useCase.ExecuteAsync(request);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
            _tokenGeneratorMock.Verify(g => g.GenerateToken(user), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WhenUserDoesNotExist_ThrowsInvalidCredentialsException()
        {
            // Arrange
            var request = new LoginUserRequest("notfound@example.com", "anyPassword");
            _userRepositoryMock.Setup(r => r.GetByEmailAsync(request.Email)).ReturnsAsync((User?)null);

            // Act
            var action = () => _useCase.ExecuteAsync(request);

            // Assert
            await action.Should().ThrowAsync<InvalidCredentialsException>();
            _passwordHasherMock.Verify(h => h.Verify(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WhenPasswordIsIncorrect_ThrowsInvalidCredentialsException()
        {
            // Arrange
            var request = new LoginUserRequest("test@example.com", "WrongPassword");
            var user = new User(request.Email, "hashed_password") { IsActive = true };

            _userRepositoryMock.Setup(r => r.GetByEmailAsync(request.Email)).ReturnsAsync(user);
            _passwordHasherMock.Setup(h => h.Verify(request.Password, user.PasswordHash)).Returns(false);

            // Act
            var action = () => _useCase.ExecuteAsync(request);

            // Assert
            await action.Should().ThrowAsync<InvalidCredentialsException>();
            _tokenGeneratorMock.Verify(g => g.GenerateToken(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WhenUserIsInactive_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var request = new LoginUserRequest("inactive@example.com", "CorrectPassword");
            var user = new User(request.Email, "hashed_password") { IsActive = false }; // <--- Usuario Inactivo

            _userRepositoryMock.Setup(r => r.GetByEmailAsync(request.Email)).ReturnsAsync(user);

            // Act
            var action = () => _useCase.ExecuteAsync(request);

            // Assert
            await action.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Inactive User");

            _passwordHasherMock.Verify(h => h.Verify(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}
