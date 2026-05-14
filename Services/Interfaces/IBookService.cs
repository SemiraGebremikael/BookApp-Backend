using BookApi.DTOs;
using BookApi.Models;

namespace BookApi.Services.Interfaces;

public interface IBookService
{
    Task<List<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(int id);

    Task<Book> CreateAsync(CreateBookDto dto);

    Task<Book?> UpdateAsync(  int id, UpdateBookDto dto);

    Task<bool> DeleteAsync(int id);
}