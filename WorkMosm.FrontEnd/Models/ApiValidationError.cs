namespace WorkMosm.FrontEnd.Models
{
    public record ApiValidationError(string? Title, int Status, Dictionary<string, string[]> Errors);
}
