using JobMatchBackend.Repositories;
namespace JobMatchBackend.Services;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using JobMatchBackend.Models.Entities;
using JobMatchBackend.DTOs.Response;
using JobMatchBackend.DTOs.Request;

public class AuthService : IAuthService
{
    private readonly IUserRepository UserRepository;
    private readonly IConfiguration _config;

    public AuthService(IUserRepository userRepository, IConfiguration config)
    {
        UserRepository = userRepository;
        _config = config;
    }

    public LoginResponse Login(LoginRequest request)
    {
        var user = UserRepository.GetByEmail(request.Email);

        if (user == null)
            throw new UnauthorizedAccessException("Credenciales inválidas");

        var hasher = new PasswordHasher<User>();

        var result = hasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password
        );

        if (result == PasswordVerificationResult.Failed)
            throw new UnauthorizedAccessException("Credenciales inválidas");

        var token = GenerateToken(user);

        return new LoginResponse
        {
            email = user.Email,
            UserId = user.Id.ToString(),
            Role = user.Role,
            FullName = user.FullName,
            Token = token
        };
    }

    private string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"])
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1), // ⏱️ 1 hora
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}