using JobMatchBackend.DTOs.Request;
using JobMatchBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobMatchBackend.Controllers;

[ApiController]
[Route("students")]
[Authorize]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet("{studentId}")]
    public async Task<IActionResult> GetStudentById(Guid studentId)
    {
        var callerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (callerId != studentId.ToString())
            return Forbid();

        try
        {
            var response = await _studentService.GetStudentByIdAsync(studentId);
            return Ok(response);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Student not found" });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpGet("{studentId}/skills")]
    public async Task<IActionResult> GetStudentSkills(Guid studentId)
    {
        try
        {
            var skills = await _studentService.GetStudentSkillsAsync(studentId);
            return Ok(skills);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Student not found" });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpPost("{studentId}/skills")]
    public async Task<IActionResult> AddSkillToStudent(Guid studentId, [FromBody] AddSkillRequest request)
    {
        var callerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (callerId != studentId.ToString())
            return Forbid();

        try
        {
            await _studentService.AddSkillToStudentAsync(studentId, request.SkillName);
            return StatusCode(201, new { message = "Skill added successfully" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
}