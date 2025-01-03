namespace AbstractTaskContracts.IncomeModels;

public record RestartTaskModel
{
    public required string TaskId { get; set; }
}