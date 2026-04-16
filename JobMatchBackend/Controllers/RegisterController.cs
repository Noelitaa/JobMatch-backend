using JobMatchBackend.DTOs.Request;
using JobMatchBackend.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("auth/[controller]")]
public class RegisterController : ControllerBase
{

    private readonly IUserService userService;

    public RegisterController(IUserService userService)
    {
        this.userService = userService;
    }


    [HttpPost]
    public async Task<IActionResult> RegisterStudent([FromBody] RegisterStudent request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {

            var response = await userService.CreateStudentAsync(request);
            
            return CreatedAtAction(
                nameof(RegisterStudent),
                new { id = response.Id }, 
                response
            );

        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("correo"))
        {
            return Conflict(new { message = ex.Message });
        }
    }


}