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

    public async Task<UserCompanyDto> AssignCompanyToUserAsync(int userId, int companyId, UserCompanyRole role = UserCompanyRole.Owner)
    {
        try
        {
            var existingUser = await _context.UserCompanies.FirstOrDefaultAsync(c => c.UserId == userId && c.CompanyId == companyId);
            if (existingUser != null)
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
            await _context.UserCompanies.AddAsync(userWfAdd);
            await _context.SaveChangesAsync();
            return _mapper.MapUserCompanyDto(userWfAdd);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error assigning Company with id:{companyId} to client with id:{userId}");
            throw;
        }
    }

    public async Task<UserCompanyDto> AddUserToCompanyAsync(AddUserToCompanyDto dtoint userId, int companyId, UserCompanyRole role)
    {
        try
        {
            var existingUser = await _context.UserCompanies.FirstOrDefaultAsync(c => c.UserId == userId && c.CompanyId == companyId);
            if (existingUser != null)
            {
                throw new BadHttpRequestException($"Company {companyId} is already assigned to user {userId}.");
            }
            var userWfAdd = new UserCompany
            {
                UserId = userId,
                CompanyId = companyId,
                Role = role,
                AssignedAt = DateTimeOffset.UtcNow,
                User =
                Company =
            };
            // check if Company is already associated to user
            await _context.UserCompanies.AddAsync(userWfAdd);
            await _context.SaveChangesAsync();
            return _mapper.MapUserCompanyDto(userWfAdd);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error assigning Company with id:{companyId} to client with id:{userId}");
            throw;
        }
    }

    public async Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto companyDto)
    {
        var company = _mapper.MapDtoToModel(companyDto);
        var user = await _context.Users.FindAsync(companyDto.UserId) ?? throw new KeyNotFoundException($"User with id: {companyDto.UserId} not found");

        var userCompany = _mapper.MapUserCompany(user, company);
        await _context.Companies.AddAsync(company);
        await _context.UserCompanies.AddAsync(userCompany);
        await _context.SaveChangesAsync();
        return _mapper.MapToDto(company);
    }

    public async Task DeleteCompanyAsync(int companyId)
    {
        var company = await _context.Companies.FindAsync(companyId);
        if (company == null)
        {
            _logger.LogDebug("");
            throw new KeyNotFoundException($"Company with id: {companyId} not found");
        }
        _context.Companies.Remove(company);
        await _context.SaveChangesAsync();

    }

    public async Task<List<Company>> GetCompaniesAsync()
    {
        try
        {
            var companies = await _context.Companies.ToListAsync();
            return companies;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Company> GetCompanyByIdAsync(int id)
    {
        return await _context.Companies.FindAsync(id) ?? throw new KeyNotFoundException($"Company with id: {id} not found");
    }

    public async Task<bool> SoftDeleteCompanyAsync(int companyId)
    {
        try
        {
            var company = await GetCompanyByIdAsync(companyId);
            company.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
