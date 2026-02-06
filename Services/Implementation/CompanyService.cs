using MazErpBack.Context;
using MazErpBack.DTOs.Company;
using MazErpBack.Enums;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;
using MazErpBack.Utils.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MazErpBack.Services.Implementation;

public class CompanyService(AppDbContext context, ILogger<CompanyService> logger, CompanyMapper mapper) : ICompanyService
{
    private readonly AppDbContext _context = context;
    private readonly ILogger<CompanyService> _logger = logger;
    private readonly CompanyMapper _mapper = mapper;

    public async Task<CompanyUserDto> AssignCompanyToUserAsync(int userId, int companyId, UserCompanyRole role = UserCompanyRole.Owner)
    {
        try
        {
            var existingUserWf = await _context.UserCompanies.FirstOrDefaultAsync(cw => cw.UserId == userId && cw.CompanyId == companyId);
            if (existingUserWf != null)
            {
                throw new BadHttpRequestException($"Company {companyId} is already assigned to user {userId}.");
            }
            var userWfAdd = new UserCompany
            {
                UserId = userId,
                CompanyId = companyId,
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
            _logger.LogError(ex, $"Error assigning Company with id:{companyId} to client with id:{userId}");
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

            _context.Companies.Add(Company);

            await _context.SaveChangesAsync();

            return Company;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto companyDto)
    {
        var company = _mapper.MapDtoToModel(companyDto);
        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();
        return _mapper.MapToDto(company);
    }

    public async Task<bool> DeleteCompanyAsync(int companyId)
    {
        var company = await _context.Companies.FindAsync(companyId);
        if (company == null)
        {
            _logger.LogDebug("");
            return false;
        }
        _context.Companies.Remove(company);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Company>> GetCompaniesAsync()
    {
        try
        {
            var result = await _context.Companies.ToListAsync();
            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> SoftDeleteCompanyAsync(int CompanyId)
    {
        var company = await _context.Companies.FindAsync(CompanyId);
        if (company == null)
        {
            _logger.LogDebug("No se encontró una compañía con ese id");
            return false;
        }
        company.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }
}
