using MazErpBack.Context;
using MazErpBack.DTOs.Company;
using MazErpBack.Enums;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MazErpBack.Services.Implementation;

public class CompanyService(AppDbContext context, ILogger<CompanyService> logger) : ICompanyService
{
    private readonly AppDbContext _context = context;
    private readonly ILogger<CompanyService> _logger = logger;

    public async Task<CompanyUserDto> AssignCompanyToUser(int userId, int CompanyId, UserCompanyRole role = UserCompanyRole.Admin)
    {
        try
        {
            var existingUserWf = await _context.UserCompanies.FirstOrDefaultAsync(cw => cw.UserId == userId && cw.CompanyId == CompanyId);
            if (existingUserWf != null)
            {
                throw new BadHttpRequestException($"Company {CompanyId} is already assigned to user {userId}.");
            }
            var userWfAdd = new UserCompany
            {
                UserId = userId,
                CompanyId = CompanyId,
                Role = role,
                AssignedAt = DateTimeOffset.UtcNow
            };
            // check if Company is already associated to user
            _context.UserCompanies.Add(userWfAdd);
            await _context.SaveChangesAsync();
            return new CompanyUserDto
            {
                UserId = userWfAdd.UserId,
                CompanyId = userWfAdd.CompanyId,
                Role = userWfAdd.Role,
                AssignedAt = userWfAdd.AssignedAt
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error assigning Company with id:{CompanyId} to client with id:{userId}");
            throw;
        }
    }

    public async Task<Company> CreateCompany(CreateCompanyDto CompanyDto)
    {
        try
        {
            var Company = new Company()
            {
                Name = CompanyDto.Name,
                Description = CompanyDto.Description,
                CompanyPhotoUrl = CompanyDto.CompanyPhotoUrl,
                CreatedAt = CompanyDto.CreatedAt
            };

            _context.Companys.Add(Company);

            await _context.SaveChangesAsync();

            return Company;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<List<Company>> GetCompanysAsync()
    {
        try
        {
            var result = await _context.Companys.ToListAsync();
            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
