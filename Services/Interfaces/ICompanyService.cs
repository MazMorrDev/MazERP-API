using MazErpBack.DTOs.Company;
using MazErpBack.Models;

namespace MazErpBack.Services.Interfaces;

public interface ICompanyService
{
    // Para el panel de administración
    public Task<Company> GetCompanyByIdAsync(int id);
    public Task DeleteCompanyAsync(int companyId);

    // Para el usuario común
    public Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto companyDto);
    public Task<bool> SoftDeleteCompanyAsync(int CompanyId);
    public Task<UserCompanyDto> AddUserToCompanyAsync(AddUserToCompanyDto dto);
}
