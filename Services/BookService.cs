using BookApi.DTOs;
using BookApi.Models;
using BookApi.Repositories.Interfaces;
using BookApi.Services.Interfaces;

namespace BookApi.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _repository;

    public BookService(IBookRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Book>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Book> CreateAsync(
        CreateBookDto dto)
    {
        var book = new Book
        {
            Title = dto.Title,
            Author = dto.Author,
            PublishDate = DateTime.UtcNow
        };

        await _repository.AddAsync(book);

        await _repository.SaveChangesAsync();

        return book;
    }

    public async Task<Book?> UpdateAsync(
        int id,
        UpdateBookDto dto)
    {
        var book = await _repository.GetByIdAsync(id);

        if (book == null)  return null;

        book.Title = dto.Title;
        book.Author = dto.Author;
        book.PublishDate = dto.PublishDate;

        await _repository.UpdateAsync(book);
        await _repository.SaveChangesAsync();

        return book;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var book = await _repository.GetByIdAsync(id);

        if (book == null)  return false;

        await _repository.DeleteAsync(book);

        await _repository.SaveChangesAsync();

        return true;
    }
}