using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EventBookingApp.Application.DTOs;
using EventBookingApp.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EventBookingApp.Infrastructure.Services;

public class AuthService : IAuthService
{
    private static readonly Dictionary<string, string> _users = new();
    private readonly IConfiguration _config;

    public AuthService(IConfiguration config)
    {
        _config = config;
    }

    public Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        if (_users.ContainsKey(dto.Email))
            throw new Exception("User already exists");

        _users[dto.Email] = dto.Password;
        return Task.FromResult(GenerateToken(dto.Email));
    }

    public Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        if (!_users.ContainsKey(dto.Email) || _users[dto.Email] != dto.Password)
            throw new Exception("Invalid credentials");

        return Task.FromResult(GenerateToken(dto.Email));
    }

    private AuthResponseDto GenerateToken(string email)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, email),
            new Claim(ClaimTypes.Name, email),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new AuthResponseDto { Token = new JwtSecurityTokenHandler().WriteToken(token) };
    }
}
