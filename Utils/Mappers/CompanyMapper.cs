using MazErpBack.DTOs.Company;
using MazErpBack.Models;

namespace MazErpBack.Utils.Mappers;

public class CompanyMapper : IMapper<CompanyDto, CreateCompanyDto, Company>
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
