using System.ComponentModel.DataAnnotations;

namespace AbstractTasksDal.Entities;

public class TaskEntity
{
    [Key] public required Guid Id { get; init; }

    public required string Description { get; set; }
    public required string Data { get; init; }
    public required int TTL { get; set; }
    public required DateTime Created { get; init; }
    public required string Status { get; set; }
    public required string? StatusMessage { get; set; }
    public required Guid UserId { get; set; }

    public required DateTime? ExecutionStartedAt { get; set; }
    public required DateTime? ExecutionFinishedAt { get; set; }
}