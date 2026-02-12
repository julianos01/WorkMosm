using Domain.Entities;

namespace Domain.Ports
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task<User?> GetByIdAsync(string id);
        Task UpdateAsync(User user);
    }
}
