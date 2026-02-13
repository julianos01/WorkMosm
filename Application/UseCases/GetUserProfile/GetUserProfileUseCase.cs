using Domain.CustomExceptions;
using Domain.Ports;

namespace Application.UseCases.GetUserProfile
{
    public class GetUserProfileUseCase : IGetUserProfileUseCase
    {
        private readonly IUserRepository _userRepository;

        public GetUserProfileUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<GetUserResponse> ExecuteAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Param 'email' can't be null or empty", nameof(email));

            var user = await _userRepository.GetByEmailAsync(email)
                       ?? throw new UserNotFoundException(email);

            return new GetUserResponse(Id: user.Id.ToString(), Email: user.Email);
        }
    }
}