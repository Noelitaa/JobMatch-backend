using JobMatchBackend.DTOs.Request;
using JobMatchBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobMatchBackend.Controllers;

[ApiController]
[Route("api/students")]
public class StudentSkillsController : ControllerBase
{
    private readonly ISkillService _skillService;

    public StudentSkillsController(ISkillService skillService)
    {
        _skillService = skillService;
    }

    [HttpPost("{studentId}/skills")]
    public async Task<IActionResult> AddSkill(Guid studentId, [FromBody] AddSkillRequest request)
    {
        // Validate 400 Bad Request
        if (string.IsNullOrWhiteSpace(request.SkillName))
            return BadRequest(new { message = "Skill name is required" });

        var skill = await _skillService.AddSkillToStudentAsync(studentId, request.SkillName);

        // validate 404 Not Found
        if (skill == null)
            return NotFound(new { message = "Student not found" });

        // return 201 Created
        return Created("", new { id = skill.Id, name = skill.Name });
    }
}