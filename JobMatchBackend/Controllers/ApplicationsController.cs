using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobMatchBackend.Services;

namespace JobMatchBackend.Controllers;

[ApiController]
[Route("[controller]")]
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
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    private Guid GetCurrentUserId()
    {
        return Guid.Parse("9F4AF0CA-4509-42C1-9594-BB205862F7BA");
    }
}