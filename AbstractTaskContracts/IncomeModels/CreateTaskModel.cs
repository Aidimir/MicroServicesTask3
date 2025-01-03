namespace AbstractTaskContracts.IncomeModels;

public record CreateTaskModel
{
    public string Description { get; set; } = string.Empty; // Описание задачи
    public string Data { get; set; } // Информация для обработки
    public int TTL { get; set; } // Время жизни задачи (в секундах)
}