namespace WorkMosm.Application.UseCases.RegisterUser
{
    public interface IRegisterUserUseCase
    {
        Task ExecuteAsync(RegisterUserRequest registerUserRequest);
    }
}
