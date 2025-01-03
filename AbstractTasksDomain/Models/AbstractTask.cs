namespace AbstractTasksDomain.Models;

public class AbstractTask
{
    public required string Id { get; set; }
    public required string Description { get; set; } = string.Empty; // Описание задачи
    public required string Data { get; set; } // Информация для обработки
    public required int TTL { get; set; } // Время жизни задачи (в секундах)
    public required string Status { get; set; } = string.Empty;
    public required string? StatusMessage { get; set; } = string.Empty;
    public required DateTime? ExecutionStartedAt { get; set; }
    public required DateTime? ExecutionFinishedAt { get; set; }
}