using System.ComponentModel.DataAnnotations;

namespace MazErpAPI.DTOs.Inventory;

public record class CreateWarehouseDto
{
    // Foreign Key para Company
    [Required]
    public int CompanyId { get; init; }

    [Required, MaxLength(30)]
    public required string Name { get; init; }

    [MaxLength(255)]
    public string? Description { get; init; }

    // Notes: the DTOs that the user use must have [annotations]
    // the ones that are sended its unnecesary to use annotations
}
