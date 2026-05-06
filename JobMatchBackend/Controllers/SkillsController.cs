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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddSkill(Guid studentId, [FromBody] AddSkillRequest request)
    {
        // Validar 400 Bad Request
        if (string.IsNullOrWhiteSpace(request.SkillName))
            return BadRequest(new { message = "Skill name is required" });

        if (request.SkillName.Length > 100)
            return BadRequest(new { message = "Skill name is too long (max 100 characters)" });

        var skill = await _skillService.AddSkillToStudentAsync(studentId, request.SkillName.Trim());

        // Validar 404 Not Found
        if (skill == null)
            return NotFound(new { message = "Student not found" });

        // Return 201 Created
        return CreatedAtAction(nameof(AddSkill), new { studentId }, new { id = skill.Id, name = skill.Name });
    }
}