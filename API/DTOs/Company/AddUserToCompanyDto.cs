using MazErpAPI.Enums;

namespace MazErpAPI.DTOs.Company;

public record class AddUserToCompanyDto
{
    public int UserId { get; init; }
    public UserCompanyRole Role { get; init; }
}
