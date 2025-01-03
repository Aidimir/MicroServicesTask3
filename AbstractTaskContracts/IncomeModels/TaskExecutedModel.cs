using System.ComponentModel.DataAnnotations;

namespace AbstractTaskContracts.IncomeModels;

public record TaskExecutedModel : BaseAuthTaskModel
{
    [Required(ErrorMessage = "TaskId is required.")]
    [StringLength(36, ErrorMessage = "TaskId must be 36 characters long.")]
    public required string TaskId { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters.")]
    public required string Status { get; set; }

    [Required(ErrorMessage = "StatusMessage is required.")]
    [StringLength(500, ErrorMessage = "StatusMessage cannot exceed 500 characters.")]
    public required string StatusMessage { get; set; }

    [Required(ErrorMessage = "TTL is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "TTL must be greater than 0.")]
    public required int TTL { get; set; }

    [Required(ErrorMessage = "ExecutionStartedAt is required.")]
    public required DateTime? ExecutionStartedAt { get; set; }

    [Required(ErrorMessage = "ExecutionFinishedAt is required.")]
    public required DateTime? ExecutionFinishedAt { get; set; }
}