using MazErpAPI.Enums;

namespace MazErpAPI.DTOs.Company;

public record class UserCompanyDto
{
    public int UserId { get; init; }
    public UserCompanyRole Role { get; init; }
    public int CompanyId { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public string? CompanyPhotoUrl { get; init; }
    public Currency Currency { get; init; }
    public DateTimeOffset AssignedAt { get; init; }
    public required DateTimeOffset CompanyCreatedAt { get; init; }
}