using MazErpBack.DTOs.Company;
using MazErpBack.Enums;
using MazErpBack.Models;

namespace MazErpBack.Services.Interfaces;

public interface ICompanyService
{
    // Para el panel de administración
    public Task<List<Company>> GetCompaniesAsync();
    public Task DeleteCompanyAsync(int companyId);

    // Para el usuario común
    public Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto companyDto);
    public Task SoftDeleteCompanyAsync(int CompanyId);
    public Task<CompanyUserDto> AssignCompanyToUserAsync(int userId, int CompanyId, UserCompanyRole role = UserCompanyRole.Owner);
}
