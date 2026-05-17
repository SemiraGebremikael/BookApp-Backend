using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookApi.DTOs;
using BookApi.Models;
using BookApi.Repositories.Interfaces;
using BookApi.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace BookApi.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _repository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUserRepository repository,
        IConfiguration configuration,
        ILogger<AuthService> logger)
    {
        _repository = repository;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task RegisterAsync(RegisterDto dto)
    {
        try
        {
            _logger.LogInformation("Register started for user: {Name}", dto.Name);

            var user = new User
            {
                Name = dto.Name,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            await _repository.AddAsync(user);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Register successful for user: {Name}", dto.Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Register failed for user: {Name}", dto.Name);
            throw;
        }
    }

    public async Task<string?> LoginAsync(LoginDto dto)
    {
        try
        {
            _logger.LogInformation("Login attempt for user: {Name}", dto.Name);

            var user = await _repository.GetByUsernameAsync(dto.Name);

            if (user == null)
            {
                _logger.LogWarning("Login failed - user not found: {Name}", dto.Name);
                return null;
            }

            bool valid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);

            if (!valid)
            {
                _logger.LogWarning("Login failed - wrong password: {Name}", dto.Name);
                return null;
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name)
            };

            var keyString = _configuration["Jwt:Key"];

            if (string.IsNullOrEmpty(keyString))
            {
                _logger.LogError("JWT Key is missing in configuration");
                return null;
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            _logger.LogInformation("Login successful for user: {Name}", dto.Name);

            return jwt;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login crashed for user: {Name}", dto.Name);
            return null;
        }
    }
}