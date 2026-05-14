using BookApi.Data;
using BookApi.Models;
using BookApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookApi.Repositories;

public class QuoteRepository : IQuoteRepository
{
    private readonly AppDbContext _context;

    public QuoteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Quote>> GetAllAsync()
    {
        return await _context.Quotes.ToListAsync();
    }

    public async Task<Quote?> GetByIdAsync(int id)
    {
        return await _context.Quotes.FindAsync(id);
    }

    public async Task AddAsync(Quote quote)
    {
        await _context.Quotes.AddAsync(quote);
    }

    public async Task UpdateAsync(Quote quote)
    {
        _context.Quotes.Update(quote);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Quote quote)
    {
        _context.Quotes.Remove(quote);

        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
