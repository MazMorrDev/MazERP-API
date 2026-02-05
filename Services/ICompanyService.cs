using MazErpBack.DTOs.Company;
using MazErpBack.Enums;
using MazErpBack.Models;

namespace MazErpBack.Services;

public interface ICompanyService
{
    // Para el panel de administración
    public Task<List<Company>> GetCompaniesAsync();
    public Task DeleteCompanyAsync(int CompanyId);

    // Para el usuario común
    public Task<Company> CreateCompanyAsync(CreateCompanyDto CompanyDto);
    public Task<Company> SoftDeleteCompany(int CompanyId);
    public Task<CompanyUserDto> AssignCompanyToUserAsync(int userId, int CompanyId, UserCompanyRole role = UserCompanyRole.Owner);
}
