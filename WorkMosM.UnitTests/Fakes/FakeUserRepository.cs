using WorkMosm.Domain.Entities;
using WorkMosm.Domain.Ports;

namespace WorkMosm.UnitTests.Fakes
{
    public class FakeUserRepository : IUserRepository
    {
        private readonly Func<string, Task<User?>> _getByEmail;
        public string? LastQueriedEmail { get; private set; }
        public User? UpdatedUser { get; private set; }

        public FakeUserRepository(Func<string, Task<User?>>? getByEmail = null)
        {
            _getByEmail = getByEmail ?? (email => Task.FromResult<User?>(null));
        }

        public Task<User?> GetByEmailAsync(string email)
        {
            LastQueriedEmail = email;
            return _getByEmail(email);
        }

        public Task UpdateAsync(User user)
        {
            UpdatedUser = user;
            return Task.CompletedTask;
        }

        public Task AddAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
