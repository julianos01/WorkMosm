using FluentValidation;
using Microsoft.Extensions.Logging;
using WorkMosm.Domain.CustomExceptions;
using WorkMosm.Domain.Ports;

namespace WorkMosm.Application.UseCases.GetUserProfile
{
    public class GetUserProfileUseCase : IGetUserProfileUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<GetUserProfileUseCase> _logger;
        private readonly IValidator<GetUSerProfileRequest> _validator;

        public GetUserProfileUseCase(IUserRepository userRepository, ILogger<GetUserProfileUseCase> logger, IValidator<GetUSerProfileRequest> validator)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<GetUserResponse> ExecuteAsync(GetUSerProfileRequest request)
        {
            await _validator.ValidateAndThrowAsync(request);
            _logger.LogInformation("Getting user profile for email: {Email}", request.Email);

            var user = await _userRepository.GetByEmailAsync(request.Email)
                       ?? throw new UserNotFoundException(request.Email);
            return new GetUserResponse(Id: user.Id.ToString(), Email: user.Email);
        }
    }
}