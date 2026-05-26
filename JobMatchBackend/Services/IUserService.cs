using JobMatchBackend.DTOs.Request;
using JobMatchBackend.DTOs.Response;

namespace JobMatchBackend.Services;

public interface IUserService
{
    Task<RegisterStudentResponse> CreateStudentAsync(RegisterStudent request);
    Task<RegisterCompanyResponse> CreateCompanyAsync(RegisterCompany request);
    Task<string> SoftDeleteUserAsync(Guid authenticatedUserId, DeleteUserRequest request);
}
