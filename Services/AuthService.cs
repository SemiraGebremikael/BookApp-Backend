using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookApi.DTOs;
using BookApi.Models;
using BookApi.Repositories.Interfaces;
using BookApi.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace BookApi.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _repository;
    private readonly IConfiguration _configuration;

    public AuthService(
        IUserRepository repository,
        IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }

    public async Task RegisterAsync(RegisterDto dto)
    {
        var user = new User
        {
            Name = dto.Name,
            Password =
                BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        await _repository.AddAsync(user);

        await _repository.SaveChangesAsync();
    }

    public async Task<string?> LoginAsync(LoginDto dto)
    {
        var user =  await _repository.GetByUsernameAsync(  dto.Name);

        if (user == null)
            return null;

        bool valid =
            BCrypt.Net.BCrypt.Verify(
                dto.Password,
                user.Password);

        if (!valid)  return null;

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Name)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes( _configuration["Jwt:Key"]!));

        var credentials = new SigningCredentials( key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}