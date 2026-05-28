using JobMatchBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobMatchBackend.Controllers;

[ApiController]
[Route("contracts")]
[Authorize]
public class ContractsController : ControllerBase
{
    private readonly IContractService _contractService;

    public ContractsController(IContractService contractService)
    {
        _contractService = contractService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserContracts([FromQuery] string? status)
    {
        try
        {
            var userId = GetCurrentUserId();
            var result = await _contractService.GetContractsByUserAsync(userId, status);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{contractId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetContractById(int contractId)
    {
        try
        {
            var callerId = GetCurrentUserId();
            var result = await _contractService.GetContractByIdAsync(contractId, callerId);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { message = ex.Message });
        }
    }

    private Guid GetCurrentUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out var parsedUserId))
            throw new UnauthorizedAccessException("Invalid authenticated user");
        return parsedUserId;
    }
}
