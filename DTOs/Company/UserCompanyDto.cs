using MazErpBack.Enums;

namespace MazErpBack.DTOs.Company;

public record class UserCompanyDto
{
    public int UserId { get; init; }
    public int CompanyId { get; init; }
    public UserCompanyRole Role { get; init; } = UserCompanyRole.Spectator;
    public DateTimeOffset AssignedAt { get; init; }
}