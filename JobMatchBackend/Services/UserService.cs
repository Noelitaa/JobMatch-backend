using JobMatchBackend.DTOs.Response;
using JobMatchBackend.DTOs.Response.Student;
using JobMatchBackend.Mappers;
using JobMatchBackend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using JobMatchBackend.Models.Entities;
using JobMatchBackend.Repositories;
using JobMatchBackend.DTOs.Request;

namespace JobMatchBackend.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _dbContext;
    private readonly IUserRepository _userRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public UserService(AppDbContext dbContext, IUserRepository userRepository, IWebHostEnvironment webHostEnvironment)
    {
        _dbContext = dbContext;
        _userRepository = userRepository;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<RegisterStudentResponse> CreateStudentAsync(RegisterStudent request)
    {

        var existingUser = await _dbContext.User.FirstOrDefaultAsync(u => u.Email == request.Email && u.IsActive);

        if (existingUser != null)
        {
            throw new InvalidOperationException("El correo ya está registrado.");
        }


        var user = RegisterMapper.RegisterStudentToUser(request);

        var passwordHasher = new PasswordHasher<User>();
        user.PasswordHash = passwordHasher.HashPassword(user, request.PasswordHash);

        var createdUser = await _userRepository.AddAsync(user);

        return RegisterMapper.UserToRegisterStudentResponse(createdUser);

    }

    public async Task<RegisterCompanyResponse> CreateCompanyAsync(RegisterCompany request)
    {

        var existingUser = await _dbContext.User.FirstOrDefaultAsync(u => u.CompanyId == request.CompanyId && u.IsActive);

        if (existingUser != null)
        {
            throw new InvalidOperationException("La cedula de identificacion de esta empresa ya se encuentra registrada.");
        }

        var existingUserByEmail = await _dbContext.User.FirstOrDefaultAsync(u => u.Email == request.Email && u.IsActive);

        if (existingUserByEmail != null)
        {
            throw new InvalidOperationException("El correo electrónico ya se encuentra registrado.");
        }


        var user = RegisterMapper.RegisterCompanyToUser(request);

        var passwordHasher = new PasswordHasher<User>();
        user.PasswordHash = passwordHasher.HashPassword(user, request.PasswordHash);

        var createdUser = await _userRepository.AddCompanyAsync(user);

        return RegisterMapper.UserToRegisterCompanyResponse(createdUser);

    }

    public async Task<string> SoftDeleteUserAsync(Guid userId, Guid authenticatedUserId, DeleteUserRequest request)
    {
        if (authenticatedUserId != userId)
            throw new UnauthorizedAccessException("Authenticated user does not match the requested user");

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new KeyNotFoundException("User not found");

        if (!user.IsActive)
            throw new InvalidOperationException("Account already deleted");

        var passwordHasher = new PasswordHasher<User>();
        var passwordResult = passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password
        );

        if (passwordResult == PasswordVerificationResult.Failed)
            throw new InvalidOperationException("Incorrect password");

        var now = DateTime.UtcNow;
        user.IsActive = false;
        user.DeletedAt = now;
        user.UpdatedAt = now;
        await _userRepository.UpdateAsync(user);

        return "Account successfully deleted (soft delete)";
    }

    public async Task<UserProfileResponse> UpdateAvatarAsync(Guid userId, IFormFile avatar)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new KeyNotFoundException("User not found");

        var webRoot = _webHostEnvironment.WebRootPath
            ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        if (!string.IsNullOrEmpty(user.AvatarUrl))
        {
            var oldFilename = Path.GetFileName(user.AvatarUrl);
            var oldFilePath = Path.Combine(webRoot, "avatars", oldFilename);
            if (File.Exists(oldFilePath))
                File.Delete(oldFilePath);
        }

        var avatarsFolder = Path.Combine(webRoot, "avatars");
        Directory.CreateDirectory(avatarsFolder);

        var extension = Path.GetExtension(avatar.FileName);
        var filename = $"{userId}_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}{extension}";
        var filePath = Path.Combine(avatarsFolder, filename);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await avatar.CopyToAsync(stream);
        }

        user.AvatarUrl = $"/avatars/{filename}";
        user.UpdatedAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);

        return new UserProfileResponse
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role,
            Avatar = user.AvatarUrl,
            Phone = user.Phone,
            Bio = user.Bio,
            Active = user.IsActive
        };
    }
}
