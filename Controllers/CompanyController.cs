using MazErpBack.DTOs.Company;
using MazErpBack.Enums;
using MazErpBack.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MazErpBack.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class WfController(ICompanyService service) : ControllerBase
{
    private readonly ICompanyService _service = service;

    [HttpGet("Companys")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> GetCompanys()
    {
        var companies = await _service.GetCompaniesAsync();
        return Ok(new { data = companies });
    }

    [HttpPut("assign/{userId}/{CompanyId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignCompanyToUser(int userId, int CompanyId, [FromQuery] UserCompanyRole role = UserCompanyRole.Admin)
    {
        try
        {
            return Ok(await _service.AssignCompanyToUserAsync(userId, CompanyId, role));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, "An error occurred while assigning the Company to the user. Check logs for details or try again later.");
        }
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyDto Company)
    {
        try
        {
            return Ok(await _service.CreateCompanyAsync(Company));
        }
        catch (Exception)
        {

            throw;
        }
    }

}
