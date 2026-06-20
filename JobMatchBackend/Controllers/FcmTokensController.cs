using JobMatchBackend.DTOs.Request;
using JobMatchBackend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobMatchBackend.Controllers;

[ApiController]
[Route("notifications")]
[Authorize]
public class FcmTokensController : ControllerBase
{
    private readonly IFcmTokenRepository _fcmTokenRepository;

    public FcmTokensController(IFcmTokenRepository fcmTokenRepository)
    {
        _fcmTokenRepository = fcmTokenRepository;
    }

    [HttpPost("token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RegisterToken([FromBody] RegisterFcmTokenRequest request)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized(new { message = "Usuario autenticado inválido." });

        await _fcmTokenRepository.SaveOrUpdateAsync(userId.Value, request.Token, request.DeviceInfo);
        return Ok(new { message = "Token registrado correctamente." });
    }

    [HttpDelete("token/{token}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteToken(string token)
    {
        await _fcmTokenRepository.DeleteByTokenAsync(token);
        return NoContent();
    }

    private Guid? GetCurrentUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(userId, out var parsed) ? parsed : null;
    }
}
