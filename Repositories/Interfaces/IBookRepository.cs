using BookApi.Models;

namespace BookApi.Repositories.Interfaces
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllAsync();

        Task<Book?> GetByIdAsync(int id);

        Task AddAsync(Book book);

        Task UpdateAsync(Book book);

        Task DeleteAsync(Book book);

        Task SaveChangesAsync();
    }
}
