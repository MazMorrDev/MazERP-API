using System.ComponentModel.DataAnnotations;

namespace MazErpBack.DTOs.Company;

public record class DeleteUserCompanyDto
{
    [Required]
    public int UserId { get; init; }
    
    [Required]
    public int CompanyId { get; init; }
}
