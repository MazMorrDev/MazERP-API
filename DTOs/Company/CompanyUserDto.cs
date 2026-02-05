using MazErpBack.Enums;

namespace MazErpBack.DTOs.Company;
public record class CompanyUserDto
{
    public int UserId { get; init; }
    public int CompanyId { get; init; }
    public UserCompanyRole Role { get; init; }
    public DateTimeOffset AssignedAt { get; init; }
}