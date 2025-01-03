namespace AbstractTaskContracts.OutcomeModels;

public class TaskResponse
{
    public required string TaskId { get; set; }
    public required string Description { get; set; } = string.Empty;
    public required string Data { get; set; }
    public required int TTL { get; set; }
    public required string Status { get; set; }
    public required string StatusMessage { get; set; } 
}