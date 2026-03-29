using MazErpBack.DTOs.Company;
using MazErpBack.Models;
using MazErpBack.Utils;

namespace MazErpBack.Services.Interfaces;

public interface ICompanyService
{
    // Para el panel de administración
    public Task<Company> GetCompanyByIdAsync(int id);
    public Task DeleteCompanyAsync(int companyId);

    // Para el usuario común
    public Task<PaginatedResult<UserCompanyDto>> GetCompaniesByUser(int userId, int pageNumber, int pageSize);
    public Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto companyDto);
    public Task<bool> SoftDeleteCompanyAsync(int CompanyId);
    public Task<UserCompanyDto> AddUserToCompanyAsync(AddUserToCompanyDto dto, int companyId);
}
