using JobMatchBackend.Services;
using Microsoft.AspNetCore.Mvc;
using JobMatchBackend.DTOs.Request;
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
