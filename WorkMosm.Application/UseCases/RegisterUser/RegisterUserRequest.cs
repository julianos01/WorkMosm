namespace WorkMosm.Application.UseCases.RegisterUser
{
    public record RegisterUserRequest(string Email, string Password, string? Name, string? LastName);
}
