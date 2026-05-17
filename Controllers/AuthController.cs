using BookApi.DTOs;
using BookApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IAuthService service,
        ILogger<AuthController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        _logger.LogInformation("Register request received for: {Name}", dto.Name);

        await _service.RegisterAsync(dto);

        _logger.LogInformation("Register completed for: {Name}", dto.Name);

        return Ok(new { message = "User registered successfully" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        _logger.LogInformation("Login request received for: {Name}", dto.Name);

        var token = await _service.LoginAsync(dto);

        if (token == null)
        {
            _logger.LogWarning("Login failed for: {Name}", dto.Name);
            return Unauthorized(new { message = "Invalid credentials" });
        }

        _logger.LogInformation("Login successful for: {Name}", dto.Name);

        return Ok(new { token });
    }
}