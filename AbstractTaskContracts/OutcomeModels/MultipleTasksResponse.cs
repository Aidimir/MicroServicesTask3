namespace AbstractTaskContracts.OutcomeModels;

public record MultipleTasksResponse
{
    public required IEnumerable<TaskResponse> Tasks { get; set; }
}