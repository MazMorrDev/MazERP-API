using MazErpAPI.DTOs.Company;
using MazErpAPI.Models;
using MazErpBack.Utils;

namespace MazErpAPI.Services.Interfaces;

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
