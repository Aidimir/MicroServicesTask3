using System.ComponentModel.DataAnnotations;

namespace AbstractTaskContracts.IncomeModels;

public record BaseAuthTaskModel
{
    [Required(ErrorMessage = "UserId is required.")]
    [StringLength(36, ErrorMessage = "TaskId must be 36 characters long.")]
    public required string UserId { get; init; }
}