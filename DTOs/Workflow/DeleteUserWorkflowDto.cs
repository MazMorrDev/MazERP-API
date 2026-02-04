using System.ComponentModel.DataAnnotations;

namespace MazErpBack.DTOs.Workflow;

public record class DeleteUserWorkflowDto
{
    [Required]
    public int UserId { get; init; }
    
    [Required]
    public int WorkflowId { get; init; }
}
