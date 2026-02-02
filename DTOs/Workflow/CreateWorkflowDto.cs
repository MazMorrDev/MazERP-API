using System.ComponentModel.DataAnnotations;

namespace MazErpBack.DTOs.Workflow;

public record class CreateWorkflowDto
{
    [Required]
    [MaxLength(30)]
    public required string Name { get; init; }

    public string? Description { get; init; }

    public string? WorkflowPhotoUrl { get; init; } = null;

    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
}
