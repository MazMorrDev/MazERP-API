using System.ComponentModel.DataAnnotations;
using MazErpAPI.Enums;

namespace MazErpAPI.DTOs.Company;

public record class CreateCompanyDto
{
    [Required]
    public required int UserId { get; init; }

    [Required]
    [MaxLength(30)]
    public required string Name { get; init; }

    public string? Description { get; init; }

    [Url]
    public string? CompanyPhotoUrl { get; init; } = null;
    public Currency Currency { get; init; }

    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
}
