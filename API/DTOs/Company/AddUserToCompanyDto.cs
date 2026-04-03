using MazErpBack.Enums;

namespace MazErpBack.DTOs.Company;

public record class AddUserToCompanyDto
{
    public int UserId { get; init; }
    public UserCompanyRole Role { get; init; }
}
