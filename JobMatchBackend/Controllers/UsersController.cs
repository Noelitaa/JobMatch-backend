using System.Security.Claims;
using JobMatchBackend.DTOs.Request;
using JobMatchBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

    [HttpPut("{userId}/avatar")]
    public async Task<IActionResult> UpdateAvatar(Guid userId, [FromForm] IFormFile avatar)
    {
        try
        {
            var authenticatedUserId = GetCurrentUserId();
            if (authenticatedUserId != userId)
                return StatusCode(403, new { message = "You can only update your own avatar" });

            if (avatar == null || avatar.Length == 0)
                return BadRequest(new { message = "No file provided" });

            var result = await _userService.UpdateAvatarAsync(userId, avatar);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "User not found" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Invalid authenticated user" });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(Guid userId, [FromBody] DeleteUserRequest request)
    {
        try
        {
            var authenticatedUserId = GetCurrentUserId();
            var message = await _userService.SoftDeleteUserAsync(userId, authenticatedUserId, request);
            return Ok(new { message });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "User not found" });
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
