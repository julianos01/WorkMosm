using Microsoft.Extensions.Logging;
using Moq;
using WorkMosm.Application.Interfaces.Security;
using WorkMosm.Application.UseCases.UpdateUser;
using WorkMosm.Application.Validators;
using WorkMosm.Domain.CustomExceptions;
using WorkMosm.Domain.Entities;
using WorkMosm.Domain.Ports;

namespace WorkMosm.UnitTests.Application
{
    public class UpdateUserUseCaseTests
    {
        private readonly Mock<IUserRepository> _repositoryMock = new();
        private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
        private readonly UpdateUserProfileUseCase _sut;
        private readonly ILogger<UpdateUserProfileUseCase> _logger = new Logger<UpdateUserProfileUseCase>(new LoggerFactory());
        private readonly UpdateUserValidator _validator = new();

        public UpdateUserUseCaseTests()
        {
            _repositoryMock = new Mock<IUserRepository>();
            _sut = new UpdateUserProfileUseCase(_repositoryMock.Object, _passwordHasherMock.Object, _logger, _validator);
        }

        [Fact]
        public async Task Execute_WhenUserIsNotOwner_ShouldThrowUnauthorized()
        {
            // Arrange
            var request = new UpdateUserRequest("user-1", "test@test.com", null);
            var currentUserIdFromToken = "user-999";

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _sut.ExecuteAsync(request, currentUserIdFromToken));

            _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task Execute_WhenUserDoesNotExist_ShouldThrowUserNotFoundException()
        {
            // Arrange
            var userId = "user-123";
            var request = new UpdateUserRequest(userId, "test@test.com", null);
            _repositoryMock.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync((User?)null);

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() =>
                _sut.ExecuteAsync(request, userId));
        }

        [Fact]
        public async Task Execute_WhenPasswordIsProvided_ShouldHashNewPassword()
        {
            // Arrange
            var userId = "user-1";
            var request = new UpdateUserRequest(userId, "new@test.com", "plain-password");
            var existingUser = new User("old@test.com", "old-hash");

            _repositoryMock.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(existingUser);
            _passwordHasherMock.Setup(x => x.Hash("plain-password")).Returns("new-hashed-password");

            // Act
            await _sut.ExecuteAsync(request, userId);

            // Assert
            _passwordHasherMock.Verify(x => x.Hash("plain-password"), Times.Once);
            _repositoryMock.Verify(x => x.UpdateAsync(It.Is<User>(u =>
                u.Email == "new@test.com" &&
                u.PasswordHash == "new-hashed-password")), Times.Once);
        }

        [Fact]
        public async Task Execute_WhenPasswordIsEmpty_ShouldKeepOldPassword()
        {
            // Arrange
            var userId = "user-1";
            var request = new UpdateUserRequest(userId, "new@test.com", "");
            var existingUser = new User("old@test.com", "keep-this-hash");

            _repositoryMock.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(existingUser);

            // Act
            await _sut.ExecuteAsync(request, userId);

            // Assert
            _passwordHasherMock.Verify(x => x.Hash(It.IsAny<string>()), Times.Never);
            _repositoryMock.Verify(x => x.UpdateAsync(It.Is<User>(u =>
                u.PasswordHash == "keep-this-hash")), Times.Once);
        }
    }
}
