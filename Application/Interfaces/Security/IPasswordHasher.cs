namespace Application.Interfaces.Security
{
    public interface IPasswordHasher
    {
        bool Verify(string plainTextPassword, string passwordHash);
        string Hash(string plainTextPassword);
    }
}
