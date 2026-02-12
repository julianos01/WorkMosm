
using Domain.Entities;
using System.Threading.Tasks;

namespace Application.UseCases.GetUserProfile
{
    public interface IGetUserProfileUseCase
    {
        Task<GetUserResponse> GetUserProfileAsync(string email);
    }
}   