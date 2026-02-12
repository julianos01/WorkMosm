using System.Threading.Tasks;

namespace Application.UseCases.DeactivateUser
{
    public interface IDeactivateUserUseCase
    {
        Task ExecuteAsync(string email);
    }
}