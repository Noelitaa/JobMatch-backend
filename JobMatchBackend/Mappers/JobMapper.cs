using JobMatchBackend.DTOs.Request;
using JobMatchBackend.DTOs.Response;
using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Mappers;

public static class JobMapper
{
    public static Job ToEntity(CreateJobRequest Dto)
    {
        return new Job
        {
            IdCompany = Guid.TryParse(dto.CompanyId, out var id) ? id : Guid.Empty,
            Title = dto.Title,
            Description = dto.Description,
            Payment = dto.Payment,
            PaymentType = dto.PaymentType,
            WorkDate = DateOnly.TryParse(dto.Date, out var date) ? date : DateOnly.FromDateTime(DateTime.UtcNow),
            StartTime = TimeOnly.TryParse(dto.StartTime, out var start) ? start : TimeOnly.MinValue,
            EndTime = TimeOnly.TryParse(dto.EndTime, out var end) ? end : TimeOnly.MinValue,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Deliverables = dto.Deliverables != null ? string.Join(",", dto.Deliverables) : null,
            Type = "fixed-time",
            Status = "open"
        };
    }

    public static JobResponse ToResponse(Job Job)
    {
        return new JobResponse
        {
            IdJob = Job.IdJob,
            IdCompany = Job.IdCompany,
            Title = Job.Title,
            Type = Job.Type,
            Status = Job.Status,
            Payment = Job.Payment,
            PaymentType = Job.PaymentType,
            WorkDate = Job.WorkDate,
            StartTime = Job.StartTime,
            EndTime = Job.EndTime,
            CreatedAt = Job.CreatedAt
        };
    }
}
