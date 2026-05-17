using BookApi.DTOs;
using BookApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _service;
    private readonly ILogger<BooksController> _logger;

    public BooksController(
        IBookService service,
        ILogger<BooksController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GET all books request");

        var books = await _service.GetAllAsync();

        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GET book by ID: {Id}", id);

        var book = await _service.GetByIdAsync(id);

        if (book == null)
        {
            _logger.LogWarning("Book not found: {Id}", id);
            return NotFound(new { message = "Book not found" });
        }

        return Ok(book);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBookDto dto)
    {
        _logger.LogInformation("CREATE book request: {Title}", dto.Title);

        var created = await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = created.Id },
            created
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateBookDto dto)
    {
        _logger.LogInformation("UPDATE book request: {Id}", id);

        var result = await _service.UpdateAsync(id, dto);

        if (result == null)
        {
            _logger.LogWarning("Update failed - book not found: {Id}", id);
            return NotFound(new { message = "Book not found" });
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("DELETE book request: {Id}", id);

        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
        {
            _logger.LogWarning("Delete failed - book not found: {Id}", id);
            return NotFound(new { message = "Book not found" });
        }

        return NoContent();
    }
}