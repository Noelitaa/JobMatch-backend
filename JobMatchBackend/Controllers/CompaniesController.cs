// Controllers/CompaniesController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobMatchBackend.Services;

namespace JobMatchBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize] // Cualquier usuario autenticado puede ver
public class CompaniesController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompaniesController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    
   [HttpGet("{companyId}")]
public async Task<IActionResult> GetCompanyProfile(Guid companyId)
{
    try
    {
        var profile = await _companyService.GetCompanyProfileAsync(companyId);
        return Ok(profile);
    }
    catch (KeyNotFoundException ex)
    {
        return NotFound(new { message = ex.Message });
    }
    catch (Exception ex)  // 👈 Capturar el error real
    {
        return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
    }
}
}