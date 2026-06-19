using System.Text.Json;
using JobMatchBackend.DTOs.Request;
using JobMatchBackend.DTOs.Response;
using JobMatchBackend.Mappers;
using JobMatchBackend.Models.Entities;
using JobMatchBackend.Repositories;

namespace JobMatchBackend.Services;

public class JobService : IJobService
{
    private readonly IJobRepository _jobRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IStudentRepository _studentRepository;

    public JobService(IJobRepository jobRepository, INotificationRepository notificationRepository, ICompanyRepository companyRepository, IStudentRepository studentRepository)
    {
        _jobRepository = jobRepository;
        _notificationRepository = notificationRepository;
        _companyRepository = companyRepository;
        _studentRepository = studentRepository;
    }

    public async Task<JobResponse> CreateJobAsync(Guid companyId, CreateJobRequest request)
    {
        var company = await _companyRepository.GetCompanyByIdAsync(companyId);
        if (company == null)
            throw new KeyNotFoundException("Empresa no encontrada.");

        if (!company.IsActive)
            throw new UnauthorizedAccessException("La empresa no está activa.");

        if (company.Role != "Company")
            throw new UnauthorizedAccessException("Solo las empresas pueden publicar ofertas.");

        var errors = new List<string>();
        TimeOnly startTime = TimeOnly.MinValue;
        TimeOnly endTime = TimeOnly.MinValue;

        var allowedTypes = new[] { "autonomous", "fixed-time" };
        if (string.IsNullOrWhiteSpace(request.Type) ||
            !allowedTypes.Contains(request.Type, StringComparer.OrdinalIgnoreCase))
            errors.Add("El tipo de trabajo es requerido y debe ser 'autonomous' o 'fixed-time'.");

        var isAutonomous = string.Equals(request.Type, "autonomous", StringComparison.OrdinalIgnoreCase);

        if (string.IsNullOrWhiteSpace(request.Title))
            errors.Add("El título es requerido.");

        if (request.Payment <= 0)
            errors.Add("El monto debe ser mayor a 0.");

        var allowedPaymentTypes = new[] { "hora", "turno", "proyecto" };
        if (string.IsNullOrWhiteSpace(request.PaymentType) ||
            !allowedPaymentTypes.Contains(request.PaymentType, StringComparer.OrdinalIgnoreCase))
            errors.Add("La modalidad de pago debe ser 'hora', 'turno' o 'proyecto'.");

        if (isAutonomous)
        {
            if (request.StartDate == null)
                errors.Add("La fecha de inicio es requerida.");

            if (request.EndDate == null)
                errors.Add("La fecha de fin es requerida.");

            if (request.Deliverables == null || request.Deliverables.Count == 0)
                errors.Add("Debes agregar al menos un entregable.");
        }
        else
        {
            if (string.IsNullOrWhiteSpace(request.Date) || !DateOnly.TryParse(request.Date, out _))
                errors.Add("La fecha del trabajo es requerida y debe tener el formato YYYY-MM-DD.");

            if (string.IsNullOrWhiteSpace(request.StartTime) || !TimeOnly.TryParse(request.StartTime, out startTime))
                errors.Add("La hora de inicio es requerida y debe tener el formato HH:mm.");

            if (string.IsNullOrWhiteSpace(request.EndTime) || !TimeOnly.TryParse(request.EndTime, out endTime))
                errors.Add("La hora de fin es requerida y debe tener el formato HH:mm.");
        }

        if (errors.Count > 0)
            throw new ArgumentException(string.Join(" ", errors));

        if (isAutonomous)
        {
            if (request.StartDate!.Value > request.EndDate!.Value)
                throw new ArgumentException("La fecha de inicio debe ser anterior o igual a la fecha de fin.");

            if (request.EndDate.Value < DateOnly.FromDateTime(DateTime.UtcNow))
                throw new ArgumentException("La fecha de fin debe ser en el futuro.");
        }
        else
        {
            if (DateTime.TryParse(request.Date + " " + request.StartTime, out var jobDateTime))
            {
                if (jobDateTime <= DateTime.UtcNow)
                    throw new ArgumentException("La fecha y hora del trabajo deben ser en el futuro.");
            }

            if (startTime >= endTime)
                throw new ArgumentException("La hora de inicio debe ser anterior a la hora de fin.");
        }

        var job = JobMapper.ToEntity(request);
        job.IdCompany = companyId; // authoritative source: JWT, never the request body
        var created = await _jobRepository.CreateAsync(job);
        return JobMapper.ToResponse(created);
    }

    public async Task<JobDetailResponse> GetJobByIdAsync(int jobId)
    {
        var job = await _jobRepository.GetByIdWithCompanyAsync(jobId);
        if (job == null)
            throw new KeyNotFoundException("Oferta no encontrada.");

        return new JobDetailResponse
        {
            IdJob = job.IdJob,
            IdCompany = job.IdCompany,
            Title = job.Title,
            Description = job.Description,
            Type = job.Type ?? string.Empty,
            Status = job.Status,
            Payment = job.Payment,
            PaymentType = job.PaymentType,
            WorkDate = job.WorkDate,
            StartTime = job.StartTime,
            EndTime = job.EndTime,
            StartDate = job.StartDate,
            EndDate = job.EndDate,
            Deliverables = TryParseDeliverables(job.Deliverables),
            SkillsRequired = TryParseSkillsRequired(job.SkillsRequired),
            CreatedAt = job.CreatedAt,
            UpdatedAt = job.UpdatedAt,
            Company = new CompanySummaryResponse
            {
                Id = job.Company?.Id ?? Guid.Empty,
                CompanyName = job.Company?.CompanyName,
                Email = job.Company?.Email ?? string.Empty,
                Phone = job.Company?.Phone,
                Description = job.Company?.Description,
                AvatarUrl = job.Company?.AvatarUrl
            }
        };
    }

    public async Task<List<JobResponse>> GetAllJobsAsync()
    {
        var jobs = await _jobRepository.GetAllAsync();
        return jobs.Select(JobMapper.ToResponse).ToList();
    }

    public async Task<JobDetailResponse> UpdateJobAsync(int jobId, Guid companyId, UpdateJobRequest request)
    {
        var job = await _jobRepository.GetByIdWithCompanyAsync(jobId);
        if (job == null)
            throw new KeyNotFoundException("Oferta no encontrada.");

        if (job.IdCompany != companyId)
            throw new UnauthorizedAccessException("No tienes permiso para modificar esta oferta.");

        var hasAcceptedApplications = await _jobRepository.HasAcceptedApplicationsAsync(jobId);
        if (hasAcceptedApplications)
            throw new InvalidOperationException("No se puede editar una oferta que ya tiene postulantes aceptados.");

        var hasActiveContract = await _jobRepository.HasActiveContractAsync(jobId);
        if (hasActiveContract)
            throw new InvalidOperationException("No se puede editar una oferta con un contrato activo.");

        if (!string.IsNullOrWhiteSpace(request.Title)) job.Title = request.Title;
        if (!string.IsNullOrWhiteSpace(request.Description)) job.Description = request.Description;
        if (request.Payment != null) job.Payment = request.Payment.Value;
        if (request.PaymentType != null) job.PaymentType = request.PaymentType;
        if (request.WorkDate != null) job.WorkDate = request.WorkDate.Value;
        if (request.StartTime != null) job.StartTime = request.StartTime.Value;
        if (request.EndTime != null) job.EndTime = request.EndTime.Value;
        if (request.StartDate != null) job.StartDate = request.StartDate;
        if (request.EndDate != null) job.EndDate = request.EndDate;
        if (request.Deliverables != null) job.Deliverables = JsonSerializer.Serialize(request.Deliverables);
        if (request.SkillsRequired != null) job.SkillsRequired = JsonSerializer.Serialize(request.SkillsRequired);

        job.UpdatedAt = DateTime.UtcNow;

        var updated = await _jobRepository.UpdateAsync(job);

        return new JobDetailResponse
        {
            IdJob = updated.IdJob,
            IdCompany = updated.IdCompany,
            Title = updated.Title,
            Description = updated.Description,
            Type = updated.Type ?? string.Empty,
            Status = updated.Status,
            Payment = updated.Payment,
            PaymentType = updated.PaymentType,
            WorkDate = updated.WorkDate,
            StartTime = updated.StartTime,
            EndTime = updated.EndTime,
            StartDate = updated.StartDate,
            EndDate = updated.EndDate,
            Deliverables = TryParseDeliverables(updated.Deliverables),
            SkillsRequired = TryParseSkillsRequired(updated.SkillsRequired),
            CreatedAt = updated.CreatedAt,
            UpdatedAt = updated.UpdatedAt,
            Company = new CompanySummaryResponse
            {
                Id = updated.Company?.Id ?? Guid.Empty,
                CompanyName = updated.Company?.CompanyName,
                Email = updated.Company?.Email ?? string.Empty,
                Phone = updated.Company?.Phone,
                Description = updated.Company?.Description,
                AvatarUrl = updated.Company?.AvatarUrl
            }
        };
    }

    public async Task CancelJobAsync(int jobId, Guid companyId)
    {
        var job = await _jobRepository.GetByIdWithCompanyAsync(jobId);
        if (job == null)
            throw new KeyNotFoundException("Oferta no encontrada.");

        if (job.IdCompany != companyId)
            throw new UnauthorizedAccessException("No tienes permiso para modificar esta oferta.");

        if (job.Status == "cancelled")
            throw new InvalidOperationException("Esta oferta ya fue cancelada.");

        var hasActiveContract = await _jobRepository.HasActiveContractAsync(jobId);
        if (hasActiveContract)
            throw new InvalidOperationException("No se puede cancelar una oferta con un contrato activo.");

        var applicantIds = await _jobRepository.GetApplicantIdsByJobIdAsync(jobId);

        job.Status = "cancelled";
        job.UpdatedAt = DateTime.UtcNow;
        await _jobRepository.CancelAsync(job);

        if (applicantIds.Count > 0)
        {
            var notifications = applicantIds.Select(studentId => new Notification
            {
                IdUser = studentId,
                Title = "Oferta cancelada",
                Body = $"La oferta \"{job.Title}\" ha sido cancelada por la empresa.",
                Type = "job_cancelled",
                Data = $"{{\"jobId\": {jobId}}}"
            }).ToList();

            await _notificationRepository.CreateManyAsync(notifications);
        }
    }

    public async Task<List<JobResponse>> GetRecommendedJobsAsync(Guid studentId)
    {
        var student = await _studentRepository.GetByIdAsync(studentId);
        if (student == null)
            throw new KeyNotFoundException("Estudiante no encontrado.");

        var openJobs = await _jobRepository.GetOpenJobsAsync();

        var studentSkillNames = student.StudentSkills?
            .Select(ss => ss.Skill?.Name ?? string.Empty)
            .ToHashSet(StringComparer.OrdinalIgnoreCase) ?? new HashSet<string>();

        var recommended = openJobs.Where(job =>
        {
            if (string.Equals(job.Type, "fixed-time", StringComparison.OrdinalIgnoreCase))
            {
                if (job.WorkDate == null || job.StartTime == null || job.EndTime == null)
                    return false;

                var dayOfWeek = (int)job.WorkDate.Value.DayOfWeek;
                return student.Availabilities != null && student.Availabilities.Any(slot =>
                    slot.DayOfWeek == dayOfWeek &&
                    slot.StartTime <= job.StartTime.Value &&
                    slot.EndTime >= job.EndTime.Value);
            }

            if (string.Equals(job.Type, "autonomous", StringComparison.OrdinalIgnoreCase))
            {
                var requiredSkills = TryParseSkillsRequired(job.SkillsRequired);
                if (requiredSkills == null || requiredSkills.Count == 0)
                    return true;

                return requiredSkills.Any(s => studentSkillNames.Contains(s));
            }

            return false;
        }).ToList();

        return recommended.Select(JobMapper.ToResponse).ToList();
    }

    private static List<string>? TryParseDeliverables(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return null;
        try { return JsonSerializer.Deserialize<List<string>>(raw); }
        catch { return null; }
    }

    private static List<string>? TryParseSkillsRequired(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return null;
        try { return JsonSerializer.Deserialize<List<string>>(raw); }
        catch { return null; }
    }
}