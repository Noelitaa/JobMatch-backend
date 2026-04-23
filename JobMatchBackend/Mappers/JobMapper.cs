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
            IdCompany = int.TryParse(Dto.CompanyId, out var id) ? id : 0,
            Title = Dto.Title,
            Description = Dto.Description,
            Payment = Dto.Payment,
            PaymentType = Dto.PaymentType,
            WorkDate = DateOnly.TryParse(Dto.Date, out var date) ? date : DateOnly.FromDateTime(DateTime.UtcNow),
            StartTime = TimeOnly.TryParse(Dto.StartTime, out var start) ? start : TimeOnly.MinValue,
            EndTime = TimeOnly.TryParse(Dto.EndTime, out var end) ? end : TimeOnly.MinValue,
            StartDate = Dto.StartDate,
            EndDate = Dto.EndDate,
            Deliverables = Dto.Deliverables != null ? string.Join(",", Dto.Deliverables) : null,
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