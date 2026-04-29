using JobMatchBackend.DTOs.Response;
using JobMatchBackend.Mappers;
using JobMatchBackend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using JobMatchBackend.Models.Entities;
using JobMatchBackend.Repositories;
using JobMatchBackend.DTOs.Request;

namespace JobMatchBackend.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _dbContext;
    private readonly IUserRepository _userRepository;

    public UserService(AppDbContext dbContext, IUserRepository userRepository)
    {
        _dbContext = dbContext;
        _userRepository = userRepository;
    }

    public async Task<RegisterStudentResponse> CreateStudentAsync(RegisterStudent request)
    {

        var existingUser = await _dbContext.User.FirstOrDefaultAsync(u => u.Email == request.Email);

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

        var existingUser = await _dbContext.User.FirstOrDefaultAsync(u => u.CompanyId == request.CompanyId);

        if (existingUser != null)
        {
            throw new InvalidOperationException("La cedula de identificacion de esta empresa ya se encuentra registrada.");
        }

        var existingUserByEmail = await _dbContext.User.FirstOrDefaultAsync(u => u.Email == request.Email);

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

}
