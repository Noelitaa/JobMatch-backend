using JobMatchBackend.DTOs.Request;
using JobMatchBackend.DTOs.Response;
using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Mappers;

public class RegisterMapper()
{
    public static User RegisterStudentToUser(RegisterStudent request)
    {
        return new User
        {
            Email = request.Email,
            PasswordHash = request.PasswordHash,
            FullName = request.FullName,
            University = request.University,
            Career = request.Career,
            StudentId = request.StudentId,
            CreatedAt = DateTime.UtcNow,
            Role = "Student"
        };
    }

    public static RegisterStudentResponse UserToRegisterStudentResponse(User user)
    {
        return new RegisterStudentResponse
        {
            Id = user.Id,
            Message = "Registro de estudiante exitoso"
        };
    }

    public static User RegisterCompanyToUser(RegisterCompany request)
    {
        return new User
        {
            Email = request.Email,
            PasswordHash = request.PasswordHash,
            CompanyName = request.CompanyName,
            Description = request.Description,
            CompanyId = request.CompanyId,
            CreatedAt = DateTime.UtcNow,
            Role = "Company",
            Phone = request.Phone
        };
    }   

    public static RegisterCompanyResponse UserToRegisterCompanyResponse(User user)
    {
        return new RegisterCompanyResponse
        {
            Id = user.Id,
            Message = "Registro de empresa exitoso"
        };
    }

}