using Application.Interfaces.Security;


namespace Infrastructure.Security
{
    internal class BCryptPasswordHasher : IPasswordHasher
    {
        public string Hash(string plainTextPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainTextPassword);
        }

        public bool Verify(string plainTextPassword, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(plainTextPassword, passwordHash);
        }
    }
}
