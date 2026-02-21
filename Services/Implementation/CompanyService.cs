using MazErpBack.Context;
using MazErpBack.DTOs.Company;
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

    public async Task<UserCompanyDto> AddUserToCompanyAsync(AddUserToCompanyDto dto)
    {
        try
        {
            // check if Company is already associated to user
            var existingUser = await _context.UserCompanies.FirstOrDefaultAsync(c => c.UserId == dto.UserId && c.CompanyId == dto.CompanyId);
            if (existingUser != null)
            {
                throw new BadHttpRequestException($"Company {dto.CompanyId} is already assigned to user {dto.UserId}.");
            }

            var user = await _context.Users.FindAsync(dto.UserId) ?? throw new KeyNotFoundException($"User with id: {dto.UserId} not found");
            var company = await GetCompanyByIdAsync(dto.CompanyId);
            var userCompany = new UserCompany
            {
                UserId = dto.UserId,
                CompanyId = dto.CompanyId,
                Role = dto.Role,
                AssignedAt = DateTimeOffset.UtcNow,
                User = user,
                Company = company
            };

            await _context.UserCompanies.AddAsync(userCompany);
            await _context.SaveChangesAsync();
            return _mapper.MapUserCompanyDto(userCompany);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error assigning Company with id:{dto.CompanyId} to client with id:{dto.UserId}");
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

    public async Task<Company> GetCompanyByIdAsync(int id)
    {
        var company = await _context.Companies.FindAsync(id);
        if (company == null || !company.IsActive) throw new KeyNotFoundException($"Company with id: {id} not found");
        return company;
    }

    public async Task<bool> SoftDeleteCompanyAsync(int companyId)
    {
        try
        {
            var company = await GetCompanyByIdAsync(companyId);
            var UserCompanies = await _context.UserCompanies.Where(uc => uc.CompanyId == companyId).ToListAsync();
            foreach (var item in UserCompanies)
            {
                item.IsActive = false;
            }
            company.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task DeleteCompanyAsync(int companyId)
    {
        var company = await GetCompanyByIdAsync(companyId);
        _context.Companies.Remove(company);
        await _context.SaveChangesAsync();
    }

    public async Task<List<CompanyDto>> GetCompaniesByUser(int userId)
    {
        var userCompanies = await _context.UserCompanies.Where(uc => uc.UserId == userId).ToListAsync();
        List<CompanyDto> companiesDto = [];
        foreach (var item in userCompanies)
        {
            var company = await GetCompanyByIdAsync(item.CompanyId);
            companiesDto.Add(_mapper.MapToDto(company));
        }
        ;
        return companiesDto;
    }
}
