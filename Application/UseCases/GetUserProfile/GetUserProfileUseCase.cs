using System;
using System.Threading.Tasks;
using Application.UseCases.GetUserProfile;
using Domain.CustomExceptions;
using Domain.Entities;
using Domain.Ports;

namespace Application.UseCases.UpdateUserProfile
{
    public class GetUserProfileUseCase : IGetUserProfileUseCase
    {
        private readonly IUserRepository _userRepository;

        public GetUserProfileUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<GetUserResponse> GetUserProfileAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El parámetro 'email' no puede ser nulo o vacío.", nameof(email));

            var user = await _userRepository.GetByEmailAsync(email) 
                       ?? throw new UserNotFoundException(email); 

            return new GetUserResponse(Id: user.Id.ToString(), Email: user.Email);
        }
    }
}