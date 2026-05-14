using BookApi.DTOs;
using BookApi.Models;
using BookApi.Repositories.Interfaces;
using BookApi.Services.Interfaces;

namespace BookApi.Services;

public class QuoteService : IQuoteService
{
    private readonly IQuoteRepository _repository;

    public QuoteService(IQuoteRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Quote>> GetAllAsync()
    {
        var quotes = await _repository.GetAllAsync();

        if (quotes.Count == 0)
        {
            foreach (var text in SeedQuotes.Quotes)
            {
                await _repository.AddAsync(new Quote { Text = text });
            }

            await _repository.SaveChangesAsync();
            quotes = await _repository.GetAllAsync();
        }

        return quotes;
    }

    public async Task<Quote> CreateAsync(CreateQuoteDto dto)
    {
        var quote = new Quote
        {
            Text = dto.Text
        };

        await _repository.AddAsync(quote);

        await _repository.SaveChangesAsync();

        return quote;
    }

    public async Task<Quote?> UpdateAsync(int id, UpdateQuoteDto dto)
    {
        var quote = await _repository.GetByIdAsync(id);
        if (quote == null) return null;

        quote.Text = dto.Text;

        await _repository.UpdateAsync(quote);
        await _repository.SaveChangesAsync();

        return quote;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var quote = await _repository.GetByIdAsync(id);

        if (quote == null) return false;

        await _repository.DeleteAsync(quote);

        await _repository.SaveChangesAsync();

        return true;
    }
}

