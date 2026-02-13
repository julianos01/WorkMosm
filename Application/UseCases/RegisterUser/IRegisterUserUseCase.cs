namespace Application.UseCases.RegisterUser
{
    public interface IRegisterUserUseCase
    {
        Task ExecuteAsync(RegisterUserRequest registerUserRequest);
    }
}
