using MazErpBack.Enums;

namespace MazErpBack.DTOs.Company;

public record class UserCompanyDto
{
    public int UserId { get; init; }
    public int CompanyId { get; init; }
    public UserCompanyRole UserCompanyRole { get; init; } = UserCompanyRole.Spectator;
}
