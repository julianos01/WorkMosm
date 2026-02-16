using FluentValidation;
using Microsoft.Extensions.Logging;
using WorkMosm.Domain.CustomExceptions;
using WorkMosm.Domain.Ports;

namespace WorkMosm.Application.UseCases.DeactivateUser
{
    public class DeactivateUserUseCase : IDeactivateUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<DeactivateUserUseCase> _logger;
        private readonly IValidator<DeactivateUserRequest> _validator;

        public DeactivateUserUseCase(IUserRepository userRepository, IValidator<DeactivateUserRequest> validator, ILogger<DeactivateUserUseCase> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task ExecuteAsync(DeactivateUserRequest request)
        {
            await _validator.ValidateAndThrowAsync(request);
            _logger.LogInformation("Starting deactivation process for user with email: {Email}", request.Email);

            var user = await _userRepository.GetByEmailAsync(request.Email)
                       ?? throw new UserNotFoundException(request.Email);

            user.Deactivate();

            await _userRepository.UpdateAsync(user);
        }
    }
}