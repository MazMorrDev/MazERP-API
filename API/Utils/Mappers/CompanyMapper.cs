using MazErpAPI.DTOs.Company;
using MazErpAPI.Enums;
using MazErpAPI.Models;

namespace MazErpAPI.Utils.Mappers;

public class CompanyMapper
{
    public CompanyDto MapToDto(Company company)
    {
        return new CompanyDto
        {
            Id = company.Id,
            Name = company.Name,
            Description = company.Description,
            CompanyPhotoUrl = company.CompanyPhotoUrl,
            Currency = company.Currency,
            CreatedAt = company.CreatedAt
        };
    }

    public UserCompanyDto MapUserCompanyDto(UserCompany userCompany)
    {
        return new UserCompanyDto
        {
            UserId = userCompany.UserId,
            Name = userCompany.Company.Name,
            CompanyId = userCompany.CompanyId,
            Role = userCompany.Role,
            AssignedAt = userCompany.AssignedAt,
            CompanyCreatedAt = userCompany.Company.CreatedAt,
            Currency = userCompany.Company.Currency,
            Description = userCompany.Company.Description,
            CompanyPhotoUrl = userCompany.Company.CompanyPhotoUrl,
        };
    }

    public List<UserCompanyDto> MapUserCompanyListToDto(List<UserCompany> userCompanies)
    {
        List<UserCompanyDto> userCompaniesDto = [];
        foreach (var item in userCompanies)
        {
            userCompaniesDto.Add(MapUserCompanyDto(item));
        }
        return userCompaniesDto;
    }

    public UserCompany MapUserCompany(User user, Company company)
    {
        return new UserCompany
        {
            UserId = user.Id,
            CompanyId = company.Id,
            Role = UserCompanyRole.Owner,
            AssignedAt = DateTimeOffset.UtcNow,
            User = user,
            Company = company
        };
    }

    public Company MapDtoToModel(CreateCompanyDto companyDto)
    {
        return new Company
        {
            Name = companyDto.Name,
            Description = companyDto.Description,
            CompanyPhotoUrl = companyDto.CompanyPhotoUrl,
            Currency = companyDto.Currency,
            CreatedAt = companyDto.CreatedAt
        };
    }

    public List<CompanyDto> MapListToDto(List<Company> companies)
    {
        List<CompanyDto> companiesDto = [];
        foreach (var company in companies)
        {
            companiesDto.Add(MapToDto(company));
        }
        return companiesDto;
    }
}
