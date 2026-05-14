using BookApi.DTOs;
using BookApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        RegisterDto dto)
    {
        await _service.RegisterAsync(dto);

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(  LoginDto dto)
    {
        var token =
            await _service.LoginAsync(dto);

        if (token == null)
            return Unauthorized();

        return Ok(new { token });
    }
}