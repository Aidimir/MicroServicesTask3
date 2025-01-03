namespace AbstractTaskContracts.OutcomeModels;

public record DeleteTaskResponse
{
    public string Message { get; set; }
    public bool Success { get; set; }
}