using Application.UseCases.LoginUser.Records;

namespace Application.UseCases.LoginUser
{
    public interface ILoginUserUseCase
    {
        Task<LoginUserResult> ExecuteAsync(LoginUserRequest request);

    }
}
