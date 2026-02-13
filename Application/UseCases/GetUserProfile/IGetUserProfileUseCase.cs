namespace Application.UseCases.GetUserProfile
{
    public interface IGetUserProfileUseCase
    {
        Task<GetUserResponse> ExecuteAsync(string email);
    }
}