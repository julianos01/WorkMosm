namespace Application.UseCases.UpdateUser
{
    public interface IUpdateUserProfile
    {
        Task ExecuteAsync(UpdateUserRequest request, string? currentUserId);
    }
}
