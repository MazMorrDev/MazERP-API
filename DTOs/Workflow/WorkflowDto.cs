using MazErpBack.Enums;

namespace MazErpBack.DTOs.Workflow;

public record class WorkflowDto
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public string? WorkflowPhotoUrl { get; init; }
    public Currency Currency { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
}
