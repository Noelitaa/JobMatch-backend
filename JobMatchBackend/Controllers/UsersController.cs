using System.Security.Claims;
using JobMatchBackend.DTOs.Request;
using JobMatchBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobMatchBackend.Controllers;

[ApiController]
[Route("users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(Guid userId, [FromBody] DeleteUserRequest request)
    {
        var authenticatedUserId = GetCurrentUserId();
        if (authenticatedUserId != userId)
            return StatusCode(403, new { message = "Authenticated user does not match the requested user" });

        try
        {
            var message = await _userService.DeleteUserAsync(userId, request);
            return Ok(new { message });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "User not found" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return BadRequest(new { message = ex.Message });
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
