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

    [HttpGet("by-user/{userId}")]
    public async Task<IActionResult> GetCompaniesByUser(int userId)
    {
        var companies = await _service.GetCompaniesByUser(userId);
        return Ok(new { data = companies });
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AddUserToCompany([FromBody] AddUserToCompanyDto dto)
    {
        try
        {
            return Ok(await _service.AddUserToCompanyAsync(dto));
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
