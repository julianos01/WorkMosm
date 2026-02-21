namespace WorkMosm.FrontEnd.Models
{
    public record LoginResponse(string Token, DateTime Expiration, string Id);
}
