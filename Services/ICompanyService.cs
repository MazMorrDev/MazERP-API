using MazErpBack.DTOs.Company;
using MazErpBack.Enums;
using MazErpBack.Models;

namespace MazErpBack.Services;

public interface ICompanyService
{
    // Para el panel de administración
    public Task<List<Company>> GetCompaniesAsync();
    public Task<bool> DeleteCompanyAsync(int companyId);

    // Para el usuario común
    public Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto CompanyDto);
    public Task<bool> SoftDeleteCompany(int CompanyId);
    public Task<CompanyUserDto> AssignCompanyToUserAsync(int userId, int CompanyId, UserCompanyRole role = UserCompanyRole.Owner);
}
