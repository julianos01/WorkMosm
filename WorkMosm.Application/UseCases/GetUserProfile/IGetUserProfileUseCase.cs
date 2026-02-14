namespace WorkMosm.Application.UseCases.GetUserProfile
{
    public interface IGetUserProfileUseCase
    {
        Task<GetUserResponse> ExecuteAsync(GetUSerProfileRequest request);
    }
}