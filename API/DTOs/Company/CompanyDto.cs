using MazErpBack.Enums;

namespace MazErpBack.DTOs.Company;

public record class CompanyDto
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public string? CompanyPhotoUrl { get; init; }
    public Currency Currency { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
}
