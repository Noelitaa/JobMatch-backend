using JobMatchBackend.Services;
using Microsoft.AspNetCore.Mvc;

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

    [HttpDelete("{jobId}")]
    public async Task<IActionResult> DeleteJob(int jobId)
    {
        var deleted = await _jobService.DeleteJobAsync(jobId);
        if (!deleted)
            return NotFound(new { message = "Job no encontrado" });

        return Ok(new { message = "Job eliminado correctamente" });
    }
}
