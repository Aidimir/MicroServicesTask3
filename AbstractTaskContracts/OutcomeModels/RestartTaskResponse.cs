namespace AbstractTaskContracts.OutcomeModels;

public record RestartTaskResponse
{
    public required string TaskId { get; set; }
}