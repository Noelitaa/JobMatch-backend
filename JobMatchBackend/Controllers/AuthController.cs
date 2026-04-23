
using JobMatchBackend.DTOs.Request;
using JobMatchBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobMatchBackend.Controllers;


[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{

     private readonly IAuthService AuthService;

    public AuthController(IAuthService authService)
    {
        AuthService = authService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        try
        {
            var response = AuthService.Login(request);
            return Ok(response);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Credenciales inválidas" });
        }
    }
    
}