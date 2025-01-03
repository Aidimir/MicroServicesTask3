namespace AbstractTaskContracts.IncomeModels;

public record GetTaskModel
{
    public required string? TaskId { get; set; }
    public required string UserId { get; set; }
}