using System.ComponentModel.DataAnnotations;

namespace MazErpBack.DTOs.Workflow;

public record class CreateWarehouseDto
{
    // Foreign Key para Workflow
    [Required]
    public int WorkflowId { get; init; }

    [Required, MaxLength(30)]
    public required string Name { get; init; }

    [MaxLength(255)]
    public string? Description { get; init; }
}
