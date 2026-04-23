using JobMatchBackend.DTOs.Request;
using JobMatchBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
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

    [HttpPost("company")]
    public async Task<IActionResult> RegisterCompany([FromBody] RegisterCompany request)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }


        try
        {

            var response = await userService.CreateCompanyAsync(request);

            return StatusCode(201, response);

        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("cedula"))
        {
            return Conflict(new { message = ex.Message });
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("correo"))
        {
            return Conflict(new { message = ex.Message });
        }


    }


}