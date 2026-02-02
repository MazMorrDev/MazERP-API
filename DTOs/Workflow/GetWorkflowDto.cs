namespace MazErpBack.DTOs.Workflow;

public record GetWorkflowDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? WorkflowPhotoUrl { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
}
