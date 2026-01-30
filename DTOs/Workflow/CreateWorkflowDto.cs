using System.ComponentModel.DataAnnotations;

namespace MazErpBack.DTOs.Workflow;

public class CreateWorkflowDto
{
    [Required]
    [MaxLength(30)]
    public required string Name { get; set; }

    public string? Description { get; set; }

    public string? WorkflowPhotoUrl { get; set; } = null;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
