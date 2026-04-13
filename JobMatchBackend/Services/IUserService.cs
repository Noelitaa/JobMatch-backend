using JobMatchBackend.DTOs.Request;
using JobMatchBackend.DTOs.Response;

namespace JobMatchBackend.Services;

public interface IUserService
{
    Task<RegisterStudentResponse> CreateStudentAsync(RegisterStudent request);
}