using BookApi.DTOs;
using BookApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class QuotesController : ControllerBase
{
    private readonly IQuoteService _service;
    private readonly ILogger<QuotesController> _logger;

    public QuotesController(
        IQuoteService service,
        ILogger<QuotesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GET all quotes request");

        var quotes = await _service.GetAllAsync();

        return Ok(quotes);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateQuoteDto dto)
    {
        _logger.LogInformation("CREATE quote request: {Text}", dto.Text);

        var created = await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetAll),
            new { id = created.Id },
            created
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateQuoteDto dto)
    {
        _logger.LogInformation("UPDATE quote request: {Id}", id);

        var updated = await _service.UpdateAsync(id, dto);

        if (updated == null)
        {
            _logger.LogWarning("Quote not found for update: {Id}", id);
            return NotFound(new { message = "Quote not found" });
        }

        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("DELETE quote request: {Id}", id);

        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
        {
            _logger.LogWarning("Quote not found for delete: {Id}", id);
            return NotFound(new { message = "Quote not found" });
        }

        return NoContent();
    }
}