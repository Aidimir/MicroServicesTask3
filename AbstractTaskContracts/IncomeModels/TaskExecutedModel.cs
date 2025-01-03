namespace AbstractTaskContracts.IncomeModels;

public record TaskExecutedModel
{
    public required string TaskId { get; set; }
    public required string Status { get; set; }
    public required string StatusMessage { get; set; }
    public required int TTL { get; set; }
    public required DateTime? ExecutionStartedAt { get; set; }
    public required DateTime? ExecutionFinishedAt { get; set; }
}