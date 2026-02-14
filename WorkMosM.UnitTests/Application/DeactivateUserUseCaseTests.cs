using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using WorkMosm.Application.UseCases.DeactivateUser;
using WorkMosm.Application.Validators;
using WorkMosm.Domain.CustomExceptions;
using WorkMosm.Domain.Entities;
using WorkMosm.Domain.Ports;
using WorkMosm.UnitTests.Fakes;

namespace WorkMosm.UnitTests.Application
{
    public class DeactivateUserUseCaseTests
    {
        private readonly ILogger<DeactivateUserUseCase> _logger = new Logger<DeactivateUserUseCase>(new LoggerFactory());

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("invalid-email-format")]
        public async Task ExecuteAsync_InvalidEmail_ThrowsValidationException(string invalidEmail)
        {
            // Arrange
            var repoMock = new Mock<IUserRepository>();
            var validator = new DeactivateUserValidator();

            var useCase = new DeactivateUserUseCase(repoMock.Object, validator, _logger);

            var request = new DeactivateUserRequest(invalidEmail);

            // Act & Assert

            await Assert.ThrowsAsync<FluentValidation.ValidationException>(() =>
                useCase.ExecuteAsync(request));
        }

        [Fact]
        public async Task ExecuteAsync_UserNotFound_ThrowsUserNotFoundException()
        {
            // Arrange
            var validator = new DeactivateUserValidator();
            var repo = new FakeUserRepository(email => Task.FromResult<User?>(null));
            var useCase = new DeactivateUserUseCase(repo, validator, _logger);
            var testEmail = "notfound@example.com";

            // Act & Assert
            var request = new DeactivateUserRequest(testEmail);
            await Assert.ThrowsAsync<UserNotFoundException>(() => useCase.ExecuteAsync(request));

            Assert.Equal(testEmail, repo.LastQueriedEmail);
        }

        [Fact]
        public async Task ExecuteAsync_SuccessfulDeactivation_UpdatesUser()
        {
            // Arrange
            var testEmail = "user@example.com";
            var request = new DeactivateUserRequest(testEmail);

            var user = new User(testEmail, "some_hash") { IsActive = true };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(r => r.GetByEmailAsync(testEmail))
                              .ReturnsAsync(user);

            var validator = new DeactivateUserValidator();

            var useCase = new DeactivateUserUseCase(userRepositoryMock.Object, validator, _logger);

            // Act
            await useCase.ExecuteAsync(request);

            // Assert
            user.IsActive.Should().BeFalse();

            userRepositoryMock.Verify(r => r.UpdateAsync(It.Is<User>(u => u.Email == testEmail && !u.IsActive)), Times.Once);
        }
    }
}
