using Application.Interfaces.Security;
using Application.UseCases.RegisterUser;
using Application.Validators;
using Domain.Entities;
using Domain.Ports;
using FluentAssertions;
using FluentValidation;
using Moq;

namespace WorkMosM.UnitTests.Application
{
    public class RegisterUserUseCaseTests
    {
        private readonly RegisterUserValidator _validator;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly RegisterUserUseCase _useCase;

        public RegisterUserUseCaseTests()
        {
            _validator = new RegisterUserValidator();
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();

            _useCase = new RegisterUserUseCase(
                _userRepositoryMock.Object,
                _passwordHasherMock.Object,
                _validator);
        }

        [Fact]
        public async Task ExecuteAsync_WhenUserDoesNotExist_AddsUserWithHashedPassword()
        {
            // Arrange
            var request = new RegisterUserRequest("test@example.com", "P@ssw0rd123");
            var hashedPassword = "hashedValue";

            _userRepositoryMock
                .Setup(r => r.GetByEmailAsync(request.Email))
                .ReturnsAsync((User?)null);

            _passwordHasherMock
                .Setup(h => h.Hash(request.Password))
                .Returns(hashedPassword);

            // Act
            await _useCase.ExecuteAsync(request);

            // Assert
            _passwordHasherMock.Verify(h => h.Hash(request.Password), Times.Once);
            _userRepositoryMock.Verify(r => r.AddAsync(It.Is<User>(u =>
                u.Email == request.Email && u.PasswordHash == hashedPassword)), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WhenUserAlreadyExists_ThrowsInvalidOperationException()
        {
            // Arrange
            var request = new RegisterUserRequest("exists@example.com", "anyPassword");
            var existingUser = new User(request.Email, "existingHash");

            _userRepositoryMock
                .Setup(r => r.GetByEmailAsync(request.Email))
                .ReturnsAsync(existingUser);

            // Act
            var action = () => _useCase.ExecuteAsync(request);

            // Assert 
            await action.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("User already exists.");

            _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WhenRequestIsInvalid_ThrowsValidationException()
        {
            // Arrange
            var request = new RegisterUserRequest("email-invalido", "123");

            // Act
            var action = () => _useCase.ExecuteAsync(request);

            // Assert
            await action.Should().ThrowAsync<ValidationException>();

            _userRepositoryMock.Verify(r => r.GetByEmailAsync(It.IsAny<string>()), Times.Never);
        }
    }
}
