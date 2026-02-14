namespace WorkMosm.Application.UseCases.UpdateUser
{
    public record UpdateUserRequest(string Id, string? Email, string? Password);
}
