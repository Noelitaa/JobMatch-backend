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
            WorkDate = DateOnly.TryParse(dto.Date, out var date) ? date : null,
            StartTime = TimeOnly.TryParse(dto.StartTime, out var start) ? start : null,
            EndTime = TimeOnly.TryParse(dto.EndTime, out var end) ? end : null,
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
        return new JobResponse
        {
            IdJob = job.IdJob,
            IdCompany = job.IdCompany,
            Title = job.Title,
            Description = job.Description,
            Type = job.Type ?? string.Empty,
            Status = job.Status ?? string.Empty,
            Payment = job.Payment,
            PaymentType = job.PaymentType,
            WorkDate = job.WorkDate,
            StartTime = job.StartTime,
            EndTime = job.EndTime,
            StartDate = job.StartDate,
            EndDate = job.EndDate,
            Deliverables = TryParseDeliverables(job.Deliverables),
            CreatedAt = job.CreatedAt
        };
    }

    private static List<string>? TryParseDeliverables(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return null;
        try { return JsonSerializer.Deserialize<List<string>>(raw); }
        catch { return null; }
    }
}