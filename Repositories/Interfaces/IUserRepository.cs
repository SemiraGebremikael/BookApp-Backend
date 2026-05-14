using BookApi.Models;

namespace BookApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string name);

        Task AddAsync(User user);

        Task SaveChangesAsync();
    }
}
