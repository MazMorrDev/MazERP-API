namespace MazErpBack.Dtos.Workflow;
public class WorkflowClientDto
{
    public int ClientId { get; set; }
    public int WorkflowId { get; set; }
    public ClientWorkflowRole Role { get; set; }
    public DateTimeOffset AssignedAt { get; set; }
}