using Application.UseCases.DeactivateUser;
using Domain.CustomExceptions;
using Domain.Entities;
using System.Reflection;
using WorkMosM.UnitTests.Fakes;

namespace WorkMosM.UnitTests.Application
{
    public class DeactivateUserUseCaseTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ExecuteAsync_InvalidEmail_ThrowsArgumentException(string invalidEmail)
        {
            // Preparación
            var repo = new FakeUserRepository();
            var useCase = new DeactivateUserUseCase(repo);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync(invalidEmail));
            Assert.Equal("email", ex.ParamName);
        }

        [Fact]
        public async Task ExecuteAsync_UserNotFound_ThrowsUserNotFoundException()
        {
            // Arrange
            var repo = new FakeUserRepository(email => Task.FromResult<User?>(null));
            var useCase = new DeactivateUserUseCase(repo);
            var testEmail = "notfound@example.com";

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => useCase.ExecuteAsync(testEmail));

            Assert.Equal(testEmail, repo.LastQueriedEmail);
        }

        [Fact]
        public async Task ExecuteAsync_SuccessfulDeactivation_UpdatesUser()
        {
            // Arrange
            var testEmail = "user@example.com";

            var userType = typeof(Domain.Entities.User);
            object? userInstance = null;

            var parameterless = userType.GetConstructor(Type.EmptyTypes);
            if (parameterless != null)
            {
                userInstance = Activator.CreateInstance(userType);
            }
            else
            {
                var stringCtor = userType.GetConstructors()
                    .FirstOrDefault(c => c.GetParameters().Length == 1 && c.GetParameters()[0].ParameterType == typeof(string));
                if (stringCtor != null)
                    userInstance = stringCtor.Invoke(new object[] { testEmail });
                else
                {
                    var twoStringCtor = userType.GetConstructors()
                        .FirstOrDefault(c =>
                        {
                            var p = c.GetParameters();
                            return p.Length == 2 && p[0].ParameterType == typeof(string) && p[1].ParameterType == typeof(string);
                        });
                    if (twoStringCtor != null)
                        userInstance = twoStringCtor.Invoke(new object[] { Guid.NewGuid().ToString(), testEmail });
                }
            }

            if (userInstance == null)
                throw new InvalidOperationException("Fail creating");

            var emailProp = userType.GetProperty("Email");
            if (emailProp != null && emailProp.CanWrite)
                emailProp.SetValue(userInstance, testEmail);

            var repo = new FakeUserRepository(_ => Task.FromResult((User?)userInstance));
            var useCase = new DeactivateUserUseCase(repo);

            // Act
            await useCase.ExecuteAsync(testEmail);

            // Assert
            Assert.Same(userInstance, repo.UpdatedUser);

            var boolPropCandidates = new[] { "IsActive", "Active", "IsDeactivated", "Disabled" };
            PropertyInfo? stateProp = null;
            foreach (var name in boolPropCandidates)
            {
                var p = userType.GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (p != null && (p.PropertyType == typeof(bool) || p.PropertyType == typeof(bool?)))
                {
                    stateProp = p;
                    break;
                }
            }

            if (stateProp == null)
            {
                var field = userType.GetFields().FirstOrDefault(f => f.FieldType == typeof(bool));
                if (field != null)
                {
                    var value = (bool)field.GetValue(userInstance)!;
                    Assert.False(value, "Fail Boolean inactive");
                    return;
                }

                throw new InvalidOperationException("Fail Boolean inactive");
            }

            var stateValue = stateProp.GetValue(userInstance);
            Assert.False(Convert.ToBoolean(stateValue), $"Fail Boolean inactive");
        }
    }
}
