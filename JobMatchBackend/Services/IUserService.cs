using JobMatchBackend.DTOs.Request;
using JobMatchBackend.DTOs.Response;
using JobMatchBackend.DTOs.Response.Student;
using Microsoft.AspNetCore.Http;

namespace JobMatchBackend.Services;

public interface IUserService
{
    Task<RegisterStudentResponse> CreateStudentAsync(RegisterStudent request);
    Task<RegisterCompanyResponse> CreateCompanyAsync(RegisterCompany request);
    Task<string> SoftDeleteUserAsync(Guid userId, Guid authenticatedUserId, DeleteUserRequest request);
    Task<UserProfileResponse> UpdateAvatarAsync(Guid userId, IFormFile avatar);
}
