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

        if (!user.IsActive)
            throw new InvalidOperationException("Account inactive or deleted");

        var hasher = new PasswordHasher<User>();

        var result = hasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password
        );

        if (result == PasswordVerificationResult.Failed)
            throw new UnauthorizedAccessException("Credenciales inválidas");

        var (token, expiration) = GenerateToken(user);

        return new LoginResponse
        {
            Email = user.Email,
            UserId = user.Id,
            Role = user.Role,
            FullName = user.FullName,
            Token = token,
            Expiration = expiration
        };
    }

    private (string Token, DateTime Expiration) GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"])
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role ?? string.Empty)
        };

        var expiration = DateTime.UtcNow.AddHours(1);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return (tokenString, expiration);
    }
}
