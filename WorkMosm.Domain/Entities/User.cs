namespace WorkMosm.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string Name { get; private set; }
        public string LastName { get; private set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public User(string email, string passwordHash, string name, string lastName)
        {
            Id = Guid.NewGuid();
            Email = email;
            PasswordHash = passwordHash;
            Name = name;
            LastName = lastName;
        }

        public void UpdateProfile(string? newEmail, string? newPasswordHash)
        {
            if (!string.IsNullOrWhiteSpace(newEmail)) Email = newEmail;
            if (!string.IsNullOrWhiteSpace(newPasswordHash)) PasswordHash = newPasswordHash;
        }

        public void Deactivate() => IsActive = false;
    }
}
