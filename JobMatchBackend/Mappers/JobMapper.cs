using JobMatchBackend.DTOs.Request;
using JobMatchBackend.DTOs.Response;
using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Mappers;

public static class JobMapper
{
    public static Job ToEntity(CreateJobRequest dto)
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

    public static JobResponse ToResponse(Job job)
    {
        return new JobResponse
        {
            IdJob = job.IdJob,
            IdCompany = job.IdCompany,
            Title = job.Title,
            Type = job.Type ?? string.Empty,
            Status = job.Status ?? string.Empty,
            Payment = job.Payment,
            PaymentType = job.PaymentType,
            WorkDate = job.WorkDate,
            StartTime = job.StartTime,
            EndTime = job.EndTime,
            CreatedAt = job.CreatedAt
        };
    }
}