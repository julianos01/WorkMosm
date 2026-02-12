namespace Application.UseCases.RegisterUser
{
    public interface IRegisterUserUseCase
    {
        Task ExecuteAsync(string email, string passwordHash);
    }
}
