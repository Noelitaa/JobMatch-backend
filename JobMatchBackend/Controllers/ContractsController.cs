using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobMatchBackend.Services;
using System.Security.Claims;

namespace JobMatchBackend.Controllers;

[ApiController]
[Route("")]
[Authorize(Roles = "Student")]
public class ContractsController : ControllerBase
{
    private readonly IContractService _contractService;

    public ContractsController(IContractService contractService)
    {
        _contractService = contractService;
    }

    // PUT: /contracts/{contractId}/accept
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
        if (!Guid.TryParse(userId, out var parsedUserId))
            throw new UnauthorizedAccessException("Invalid authenticated user");
        return parsedUserId;
    }
}