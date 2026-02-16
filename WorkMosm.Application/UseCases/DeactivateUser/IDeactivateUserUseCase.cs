namespace WorkMosm.Application.UseCases.DeactivateUser
{
    public interface IDeactivateUserUseCase
    {
        Task ExecuteAsync(DeactivateUserRequest request);
    }
}