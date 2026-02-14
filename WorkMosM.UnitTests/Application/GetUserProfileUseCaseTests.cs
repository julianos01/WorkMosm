using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using WorkMosm.Application.UseCases.GetUserProfile;
using WorkMosm.Application.Validators;
using WorkMosm.Domain.CustomExceptions;
using WorkMosm.Domain.Entities;
using WorkMosm.Domain.Ports;

namespace WorkMosm.UnitTests.Application
{
    public class GetUserProfileUseCaseTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly GetUserProfileUseCase _useCase;
        private readonly ILogger<GetUserProfileUseCase> _logger = new Logger<GetUserProfileUseCase>(new LoggerFactory());
        private readonly GetUserProfileValidator _validator = new();

        public GetUserProfileUseCaseTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _useCase = new GetUserProfileUseCase(_userRepositoryMock.Object, _logger, _validator);
        }

        [Fact]
        public async Task ExecuteAsync_WhenUserExists_ReturnsUserResponse()
        {
            // Arrange
            var email = "martin@example.com";
            var user = new User(email, "anyHash");

            _userRepositoryMock
                .Setup(r => r.GetByEmailAsync(email))
                .ReturnsAsync(user);

            // Act
            var result = await _useCase.ExecuteAsync(new GetUSerProfileRequest(email));

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(email);
            result.Id.Should().Be(user.Id.ToString());

            _userRepositoryMock.Verify(r => r.GetByEmailAsync(email), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WhenUserDoesNotExist_ThrowsUserNotFoundException()
        {
            // Arrange
            var email = "notfound@example.com";
            _userRepositoryMock
                .Setup(r => r.GetByEmailAsync(email))
                .ReturnsAsync((User?)null);

            // Act
            var action = () => _useCase.ExecuteAsync(new GetUSerProfileRequest(email));

            // Assert
            await action.Should().ThrowAsync<UserNotFoundException>()
                .WithMessage($"*{email}*");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public async Task ExecuteAsync_WhenEmailIsInvalid_ThrowsValidationException(string invalidEmail)
        {
            // Act
            var action = () => _useCase.ExecuteAsync(new GetUSerProfileRequest(invalidEmail));

            // Assert
            await action.Should().ThrowAsync<ValidationException>();

            _userRepositoryMock.Verify(r => r.GetByEmailAsync(It.IsAny<string>()), Times.Never);
        }
    }
}
