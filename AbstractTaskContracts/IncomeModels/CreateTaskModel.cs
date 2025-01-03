using System.ComponentModel.DataAnnotations;

namespace AbstractTaskContracts.IncomeModels;

public record CreateTaskModel : BaseAuthTaskModel
{
    [Required(ErrorMessage = "Description is required.")]
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public required string Description { get; init; } = string.Empty; // Task description

    [Required(ErrorMessage = "Data is required.")]
    public required string Data { get; init; } // Data for processing

    [Required(ErrorMessage = "TTL is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "TTL must be greater than 0.")]
    public required int TTL { get; init; } // Time-to-live for the task (in seconds)
}
