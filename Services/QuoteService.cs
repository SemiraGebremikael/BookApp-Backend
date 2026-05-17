using BookApi.DTOs;
using BookApi.Models;
using BookApi.Repositories.Interfaces;
using BookApi.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace BookApi.Services;

public class QuoteService : IQuoteService
{
    private readonly IQuoteRepository _repository;
    private readonly ILogger<QuoteService> _logger;

    public QuoteService(
        IQuoteRepository repository,
        ILogger<QuoteService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<List<Quote>> GetAllAsync()
    {
        _logger.LogInformation("Fetching all quotes");

        var quotes = await _repository.GetAllAsync();

        if (quotes.Count == 0)
        {
            _logger.LogWarning("No quotes found - seeding default quotes");

            try
            {
                foreach (var text in SeedQuotes.Quotes)
                {
                    await _repository.AddAsync(new Quote { Text = text });
                }

                await _repository.SaveChangesAsync();

                quotes = await _repository.GetAllAsync();

                _logger.LogInformation("Seed completed. Total quotes: {Count}", quotes.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while seeding quotes");
                throw;
            }
        }

        _logger.LogInformation("Returned {Count} quotes", quotes.Count);

        return quotes;
    }

    public async Task<Quote> CreateAsync(CreateQuoteDto dto)
    {
        try
        {
            _logger.LogInformation("Creating quote: {Text}", dto.Text);

            var quote = new Quote
            {
                Text = dto.Text
            };

            await _repository.AddAsync(quote);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Quote created with ID: {Id}", quote.Id);

            return quote;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating quote");
            throw;
        }
    }

    public async Task<Quote?> UpdateAsync(int id, UpdateQuoteDto dto)
    {
        try
        {
            _logger.LogInformation("Updating quote ID: {Id}", id);

            var quote = await _repository.GetByIdAsync(id);

            if (quote == null)
            {
                _logger.LogWarning("Quote not found: {Id}", id);
                return null;
            }

            quote.Text = dto.Text;

            await _repository.UpdateAsync(quote);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Quote updated successfully: {Id}", id);

            return quote;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating quote: {Id}", id);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            _logger.LogInformation("Deleting quote ID: {Id}", id);

            var quote = await _repository.GetByIdAsync(id);

            if (quote == null)
            {
                _logger.LogWarning("Delete failed - quote not found: {Id}", id);
                return false;
            }

            await _repository.DeleteAsync(quote);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Quote deleted successfully: {Id}", id);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting quote: {Id}", id);
            throw;
        }
    }
}