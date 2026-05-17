using BookApi.DTOs;
using BookApi.Models;
using BookApi.Repositories.Interfaces;
using BookApi.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace BookApi.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _repository;
    private readonly ILogger<BookService> _logger;

    public BookService(
        IBookRepository repository,
        ILogger<BookService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<List<Book>> GetAllAsync()
    {
        _logger.LogInformation("Fetching all books");

        var books = await _repository.GetAllAsync();

        _logger.LogInformation("Fetched {Count} books", books.Count);

        return books;
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Fetching book with ID: {Id}", id);

        var book = await _repository.GetByIdAsync(id);

        if (book == null)
        {
            _logger.LogWarning("Book not found with ID: {Id}", id);
        }

        return book;
    }

    public async Task<Book> CreateAsync(CreateBookDto dto)
    {
        try
        {
            _logger.LogInformation("Creating book: {Title}", dto.Title);

            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                PublishDate = DateTime.UtcNow
            };

            await _repository.AddAsync(book);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Book created successfully with ID: {Id}", book.Id);

            return book;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while creating book: {Title}", dto.Title);
            throw;
        }
    }

    public async Task<Book?> UpdateAsync(int id, UpdateBookDto dto)
    {
        try
        {
            _logger.LogInformation("Updating book with ID: {Id}", id);

            var book = await _repository.GetByIdAsync(id);

            if (book == null)
            {
                _logger.LogWarning("Update failed - book not found: {Id}", id);
                return null;
            }

            book.Title = dto.Title;
            book.Author = dto.Author;
            book.PublishDate = dto.PublishDate;

            await _repository.UpdateAsync(book);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Book updated successfully: {Id}", id);

            return book;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating book: {Id}", id);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            _logger.LogInformation("Deleting book with ID: {Id}", id);

            var book = await _repository.GetByIdAsync(id);

            if (book == null)
            {
                _logger.LogWarning("Delete failed - book not found: {Id}", id);
                return false;
            }

            await _repository.DeleteAsync(book);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Book deleted successfully: {Id}", id);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting book: {Id}", id);
            throw;
        }
    }
}