using BookApi.DTOs;

namespace BookApi.Services.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(RegisterDto dto);

    Task<string?> LoginAsync(LoginDto dto);
}