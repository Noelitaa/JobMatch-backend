using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobMatchBackend.Services;
using JobMatchBackend.DTOs.Request;
using System.Security.Claims;

namespace JobMatchBackend.Controllers;

[ApiController]
[Route("")]
[Authorize]
public class ApplicationsController : ControllerBase
{
    private readonly IApplicationService _applicationService;

    public ApplicationsController(IApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    // POST: /applications
    [HttpPost("applications")]
    public async Task<IActionResult> CreateApplication([FromBody] CreateApplicationRequest request)
    {
        try
        {
            var studentId = GetCurrentUserId();
            var result = await _applicationService.CreateApplicationAsync(studentId, request);
            return StatusCode(201, result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    // GET: /applications/{applicationId}
    [HttpGet("applications/{applicationId}")]
    public async Task<IActionResult> GetApplicationDetails(int applicationId)
    {
        try
        {
            var userId = GetCurrentUserId();
            var userRole = GetCurrentUserRole();

            var result = await _applicationService.GetApplicationDetailsAsync(applicationId, userId, userRole);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Application not found" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    // GET: /jobs/{jobId}/applications
    [HttpGet("jobs/{jobId}/applications")]
    public async Task<IActionResult> GetApplicationsByJob(int jobId)
    {
        try
        {
            var companyId = GetCurrentUserId();
            var applications = await _applicationService.GetApplicationsByJobAsync(jobId, companyId);
            return Ok(applications);
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    // PUT: /applications/{applicationId}
    [HttpPut("applications/{applicationId}")]
    public async Task<IActionResult> UpdateApplicationStatus(int applicationId, [FromBody] UpdateApplicationRequest request)
    {
        try
        {
            var companyId = GetCurrentUserId();
            var result = await _applicationService.UpdateApplicationStatusAsync(applicationId, companyId, request);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Application not found" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    private Guid GetCurrentUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out var parsedUserId))
            throw new UnauthorizedAccessException("Invalid authenticated user");

        return parsedUserId;
    }

    private string GetCurrentUserRole()
    {
        return User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
    }
}
