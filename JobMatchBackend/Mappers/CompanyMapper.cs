// Mappers/CompanyMapper.cs
using JobMatchBackend.DTOs.Response;
using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Mappers;

public static class CompanyMapper
{
    public static CompanyProfileResponse ToProfileResponse(User company, int activeJobsCount)
    {
        return new CompanyProfileResponse
        {
            Id = company.Id,
            Email = company.Email,
            CompanyName = company.CompanyName,
            Description = company.Description,
            CompanyId = company.CompanyId,
            Phone = company.Phone,
            AvatarUrl = company.AvatarUrl,
            CreatedAt = company.CreatedAt,
            IsActive = company.IsActive,
            EmailVerified = company.EmailVerified,
            ActiveJobsCount = activeJobsCount
        };
    }

    public static CompanySummaryResponse ToSummaryResponse(User company)
    {
        return new CompanySummaryResponse
        {
            Id = company.Id,
            CompanyName = company.CompanyName,
            Email = company.Email,
            Phone = company.Phone,
            Description = company.Description,
            AvatarUrl = company.AvatarUrl
        };
    }
}
