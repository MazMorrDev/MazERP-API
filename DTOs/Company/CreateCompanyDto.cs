using System.ComponentModel.DataAnnotations;

namespace MazErpBack.DTOs.Company;

public record class CreateCompanyDto
{
    [Required]
    [MaxLength(30)]
    public required string Name { get; init; }

    public string? Description { get; init; }

    public string? CompanyPhotoUrl { get; init; } = null;

    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
}
