using BookApi.Models;

namespace BookApi.Repositories.Interfaces
{
public interface IQuoteRepository
{
    Task<List<Quote>> GetAllAsync();

    Task<Quote?> GetByIdAsync(int id);

    Task AddAsync(Quote quote);

    Task UpdateAsync(Quote quote);

    Task DeleteAsync(Quote quote);

    Task SaveChangesAsync();
}
}
