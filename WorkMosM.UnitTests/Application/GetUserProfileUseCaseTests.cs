using Application.UseCases.GetUserProfile;
using Domain.CustomExceptions;
using Domain.Entities;
using Domain.Ports;
using FluentAssertions;
using Moq;

namespace WorkMosM.UnitTests.Application
{
    public class GetUserProfileUseCaseTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly GetUserProfileUseCase _useCase;

        public GetUserProfileUseCaseTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _useCase = new GetUserProfileUseCase(_userRepositoryMock.Object);
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
            var result = await _useCase.ExecuteAsync(email);

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
            var action = () => _useCase.ExecuteAsync(email);

            // Assert
            await action.Should().ThrowAsync<UserNotFoundException>()
                .WithMessage($"*{email}*");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public async Task ExecuteAsync_WhenEmailIsInvalid_ThrowsArgumentException(string invalidEmail)
        {
            // Act
            var action = () => _useCase.ExecuteAsync(invalidEmail);

            // Assert
            await action.Should().ThrowAsync<ArgumentException>()
                .WithParameterName("email");

            _userRepositoryMock.Verify(r => r.GetByEmailAsync(It.IsAny<string>()), Times.Never);
        }
    }
}
