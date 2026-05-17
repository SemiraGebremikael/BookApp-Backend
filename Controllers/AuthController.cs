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
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        try
        {
            await _service.RegisterAsync(dto);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.ToString());
        }
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