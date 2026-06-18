using System.Text.Json;
using JobMatchBackend.DTOs.Request;
using JobMatchBackend.DTOs.Response;
using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Mappers;

public static class JobMapper
{
    // Dispatches to the correct mapping based on dto.Type.
    public static Job ToEntity(CreateJobRequest dto)
    {
        var isAutonomous = string.Equals(dto.Type, "autonomous", StringComparison.OrdinalIgnoreCase);
        return isAutonomous ? ToAutonomousEntity(dto) : ToFixedTimeEntity(dto);
    }

    // Maps a fixed-time job request to the Job entity.
    // IdCompany is intentionally left at its default; JobService sets it from the
    // authenticated JWT claim after calling this mapper, never from the request body.
    public static Job ToFixedTimeEntity(CreateJobRequest dto)
    {
        return new Job
        {
            Title = dto.Title,
            Description = dto.Description,
            Payment = dto.Payment,
            PaymentType = dto.PaymentType,
            WorkDate = DateOnly.Parse(dto.Date),
            StartTime = TimeOnly.Parse(dto.StartTime),
            EndTime = TimeOnly.Parse(dto.EndTime),
            Type = "fixed-time",
            Status = "open"
        };
    }

    // Maps an autonomous job request to the Job entity.
    // IdCompany is intentionally left at its default; JobService sets it from the
    // authenticated JWT claim after calling this mapper, never from the request body.
    public static Job ToAutonomousEntity(CreateJobRequest dto)
    {
        return new Job
        {
            Title = dto.Title,
            Description = dto.Description,
            Payment = dto.Payment,
            PaymentType = dto.PaymentType,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Deliverables = dto.Deliverables != null ? JsonSerializer.Serialize(dto.Deliverables) : null,
            Type = "autonomous",
            Status = "open"
        };
    }

    public static JobResponse ToResponse(Job job)
    {
        var isAutonomous = string.Equals(job.Type, "autonomous", StringComparison.OrdinalIgnoreCase);

        return new JobResponse
        {
            IdJob = job.IdJob,
            IdCompany = job.IdCompany,
            Title = job.Title,
            Type = job.Type ?? string.Empty,
            Status = job.Status ?? string.Empty,
            Payment = job.Payment,
            PaymentType = job.PaymentType,

            // job.WorkDate/StartTime/EndTime are non-nullable on the entity, so for
            // autonomous jobs (never set) we'd otherwise leak default values into the response.
            WorkDate = isAutonomous ? null : job.WorkDate,
            StartTime = isAutonomous ? null : job.StartTime,
            EndTime = isAutonomous ? null : job.EndTime,

            StartDate = job.StartDate,
            EndDate = job.EndDate,
            Deliverables = string.IsNullOrWhiteSpace(job.Deliverables)
                ? null
                : JsonSerializer.Deserialize<List<string>>(job.Deliverables),

            CreatedAt = job.CreatedAt
        };
    }
}