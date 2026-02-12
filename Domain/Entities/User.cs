namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get;  private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public bool IsActive { get; set; } = true;
        private User() { }
        public User(string email, string passwordHash)
        {
            Id = Guid.NewGuid();
            Email = email;
            PasswordHash = passwordHash;
        }

        public void UpdateProfile(string? newEmail, string? newPasswordHash)
        {
            if (!string.IsNullOrWhiteSpace(newEmail)) Email = newEmail;
            if (!string.IsNullOrWhiteSpace(newPasswordHash)) PasswordHash = newPasswordHash;
        }

        public void Deactivate() => IsActive = false;
    }
}
