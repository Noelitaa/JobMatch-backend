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
            IdCompany = Guid.Parse(dto.CompanyId),
            Title = dto.Title,
            Description = dto.Description,
            Payment = dto.Payment,
            PaymentType = dto.PaymentType,
            WorkDate = DateOnly.Parse(dto.Date),
            StartTime = TimeOnly.Parse(dto.StartTime),
            EndTime = TimeOnly.Parse(dto.EndTime),
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