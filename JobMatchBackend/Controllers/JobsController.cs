using JobMatchBackend.DTOs.Request;
using JobMatchBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobMatchBackend.Controllers;

[ApiController]
[Route("jobs")]
public class JobsController : ControllerBase
{
    private readonly IJobService _jobService;

    public JobsController(IJobService jobService)
    {
        _jobService = jobService;
    }

    [HttpGet("{jobId}")]
    public async Task<IActionResult> GetJobById(int jobId)
    {
        try
        {
            var response = await _jobService.GetJobByIdAsync(jobId);
            return Ok(response);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Job not found" });
        }
    }

    [Authorize]
    [HttpPut("{jobId}")]
    public async Task<IActionResult> UpdateJob(int jobId, [FromBody] UpdateJobRequest request)
    {
        try
        {
            // FIX 1: Read companyId from JWT and pass it to the service for ownership verification
            var companyIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(companyIdClaim, out var companyId))
                return Unauthorized(new { message = "Invalid or missing company identity in token" });

            var response = await _jobService.UpdateJobAsync(jobId, companyId, request);
            return Ok(response);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Job not found" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
        catch (InvalidOperationException ex)
        {
            // FIX 2: InvalidOperationException means a business rule conflict, not a permission issue
            return Conflict(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        // FIX 3: Catch-all for unexpected errors
        catch (Exception)
        {
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllJobs()
    {
        try
        {
            var jobs = await _jobService.GetAllJobsAsync();
            return Ok(jobs);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpDelete("{jobId}")]
    public async Task<IActionResult> DeleteJob(int jobId)
    {
        try
        {
            await _jobService.DeleteJobAsync(jobId);
            return Ok(new { message = "Job deleted successfully" });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Job not found" });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateJob([FromBody] CreateJobRequest request)
    {
        try
        {
            var response = await _jobService.CreateJobAsync(request);
            return StatusCode(201, new { message = "Job creado correctamente", data = response });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}