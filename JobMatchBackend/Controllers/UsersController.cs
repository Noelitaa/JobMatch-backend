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

    [HttpDelete("me")]
    public async Task<IActionResult> DeleteCurrentUser([FromBody] DeleteUserRequest request)
    {
        try
        {
            var authenticatedUserId = GetCurrentUserId();
            var message = await _userService.SoftDeleteUserAsync(authenticatedUserId, request);
            return Ok(new { message });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "User not found" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
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
