using MazErpBack.Enums;
using MazErpBack.Services;
using MazErpBack.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MazErpBack.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class WfController(ICompanyService wfService) : ControllerBase
{
    private readonly ICompanyService _wfService = wfService;

    // [HttpGet("Companys")]
    // [Authorize(Roles = "Client")]
    // public async Task<IActionResult> GetCompanys()
    // {
    //     var Companys = await _wfService.GetCompanysAsync();
    //     return Ok(new { data = Companys });
    // }

    // [HttpPut("assign/{userId}/{CompanyId}")]
    // [Authorize(Roles = "Admin")]
    // public async Task<IActionResult> AssignCompanyToUser(int userId, int CompanyId, [FromQuery] UserCompanyRole role = UserCompanyRole.Admin)
    // {
    //     try
    //     {
    //         return Ok(await _wfService.AssignCompanyToUser(userId, CompanyId, role));
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine(ex);
    //         return StatusCode(500, "An error occurred while assigning the Company to the user. Check logs for details or try again later.");
    //     }
    // }

    // [HttpPost("create")]
    // [Authorize(Roles = "Admin")] // necesito el role para crear un wf ademas del admin
    // public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyDto Company)
    // {
    //     try
    //     {
    //         return Ok(await _wfService.CreateCompany(Company));
    //     }
    //     catch (Exception)
    //     {

    //         throw;
    //     }
    // }

}
