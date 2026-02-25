using MazErpBack.DTOs.Company;
using MazErpBack.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace MazErpBack.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CompanyController(ICompanyService service, ILogger<CompanyController> logger) : ControllerBase
{
    private readonly ICompanyService _service = service;
    private readonly ILogger<CompanyController> _logger = logger;

    [HttpGet("by-user/{userId}")]
    public async Task<IActionResult> GetCompaniesByUser(int userId)
    {
        var companies = await _service.GetCompaniesByUser(userId);
        return Ok(companies);
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AddUserToCompany([FromBody] AddUserToCompanyDto dto, [FromHeader(Name = "companyId")] int companyId)
    {
        try
        {
            return Ok(await _service.AddUserToCompanyAsync(dto));
        }
        catch (KeyNotFoundException)
        {
            return NotFound("User or Company not found");
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
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (NpgsqlException ex)
        {
            return StatusCode(500, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, "An error occurred while assigning the Company to the user. Check logs for details or try again later.");
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCompany([FromHeader(Name = "companyId")] int companyId)
    {
        try
        {
            return Ok(await _service.SoftDeleteCompanyAsync(companyId));
        }
        catch (Exception)
        {
            throw;
        }
    }
}
