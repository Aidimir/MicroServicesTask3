namespace AbstractTaskContracts.IncomeModels;

public record DeleteTaskModel
{
    public required string TaskId { get; init; }
}