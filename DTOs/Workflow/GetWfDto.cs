namespace MazErpBack.Dtos.Workflow;

public class GetWfDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? WorkflowPhotoUrl { get; set; }

    public required DateTimeOffset CreatedAt { get; set; }

}
