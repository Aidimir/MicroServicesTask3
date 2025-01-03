namespace AbstractTaskContracts.OutcomeModels;

public record DeleteTaskResponse
{
    public required string Message { get; set; }
    public required bool Success { get; set; }
}