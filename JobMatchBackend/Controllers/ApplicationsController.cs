using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobMatchBackend.Services;
using JobMatchBackend.DTOs.Request;
namespace JobMatchBackend.Controllers;

[ApiController]
[Route("")]
//[Authorize]
public class ApplicationsController : ControllerBase
{
    private readonly IApplicationService _applicationService;

    public ApplicationsController(IApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    // SOLO GET - /jobs/{jobId}/applications
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
        return Guid.Parse("9F4AF0CA-4509-42C1-9594-BB205862F7BA");
    }
}