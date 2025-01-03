using System.ComponentModel.DataAnnotations;

namespace AbstractTaskContracts.IncomeModels;

public record RestartTaskModel : BaseAuthTaskModel
{
    [Required(ErrorMessage = "TaskId is required.")]
    [StringLength(36, ErrorMessage = "TaskId must be 36 characters long.")]
    public required string TaskId { get; set; }
}