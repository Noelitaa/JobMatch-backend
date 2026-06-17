using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobMatchBackend.Services;
using System.Security.Claims;

namespace JobMatchBackend.Controllers;

[ApiController]
[Route("")]
[Authorize]
public class ContractsController : ControllerBase
{
    private readonly IContractService _contractService;

    public ContractsController(IContractService contractService)
    {
        _contractService = contractService;
    }

    // GET: /contracts/{contractId}
    [HttpGet("contracts/{contractId}")]
    public async Task<IActionResult> GetContractById(int contractId)
    {
        try
        {
            var userId = GetCurrentUserId();
            var userRole = GetCurrentUserRole();
            var result = await _contractService.GetContractByIdAsync(contractId, userId, userRole);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Contract not found" });
        }
        catch (UnauthorizedAccessException ex) when (ex.Message == "Invalid authenticated user")
        {
            return Unauthorized(new { message = ex.Message });
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

    // PUT: /contracts/{contractId}/accept
    [Authorize(Roles = "Student")]
    [HttpPut("contracts/{contractId}/accept")]
    public async Task<IActionResult> AcceptContract(int contractId)
    {
        try
        {
            var studentId = GetCurrentUserId();
            var result = await _contractService.AcceptContractAsync(contractId, studentId);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Contract not found" });
        }
        catch (UnauthorizedAccessException ex) when (ex.Message == "Invalid authenticated user")
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { message = ex.Message });
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